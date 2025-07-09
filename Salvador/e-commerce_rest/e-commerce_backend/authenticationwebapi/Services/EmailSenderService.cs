using IdentityManager.DTOs;
using IdentityManager.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace IdentityManager.Services
{
    public class EmailSenderService : IEmailSender
    {

        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmailSenderService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var user = _configuration["MailSenderConfig:Username"];
            var password = _configuration["MailSenderConfig:Password"];
            var host = _configuration["MailSenderConfig:SmtpHost"];
            var smtp_port = _configuration["MailSenderConfig:SmtpPort"];
            var smtpPort = int.Parse(smtp_port!);

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse("no-reply@miapp.com"));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(host, smtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(user, password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);

        }

        public async Task<EmailSenderServiceResult> ConfirmEmail(ConfirmEmailDto dto)
        {

            if (dto.Email == null || dto.Code == null)
            {
                return new EmailSenderServiceResult
                {
                    Success = false,
                    ErrorMessage = "Email and code cannot be empty",
                    OperationDate = DateTime.Now
                };
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return new EmailSenderServiceResult
                {
                    Success = false,
                    ErrorMessage = "User not found",
                    OperationDate = DateTime.Now
                };
            }
            var result = await _userManager.ConfirmEmailAsync(user, dto.Code);
            if (result.Succeeded)
            {
                return new EmailSenderServiceResult
                {
                    Success = true,
                    ErrorMessage = "Email sent",
                    OperationDate = DateTime.Now
                };
            }

            return new EmailSenderServiceResult
            {
                Success = false,
                ErrorMessage = "Unhandled exception",
                OperationDate = DateTime.Now
            };

        }
    }
}

public class EmailSenderServiceResult
{

    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime OperationDate { get; set; }

}