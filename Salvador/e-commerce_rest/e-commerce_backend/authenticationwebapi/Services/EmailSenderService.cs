using System.Text;
using Account.DTOs.AccountDTOs;
using IdentityManager.DTOs.AuthenticationDTOs;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmailSenderService(IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _configuration = configuration;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        //Method to Send Email
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

        //Send URL to confirm email to user e-mail
        public async Task<EmailSenderServiceResult> generateEmailConfirmationUrlAndSendToEmail(RegisterDto dto)
        {

            var user = await _userManager.FindByEmailAsync(dto.Email!);
            var userId = await _userManager.GetUserIdAsync(user!);

            if (user == null || userId == null)
            {

                return new EmailSenderServiceResult
                {
                    Success = false,
                    ErrorMessage = "User cannot be null",
                    OperationDate = DateTime.Now
                };

            }

            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationUrl = GenerateConfirmationUrl(userId, confirmationToken, "confirm-email");
            Console.WriteLine($"[DEBUG] Confirm Email URL: {confirmationUrl}");

            await SendEmailAsync(dto.Email!,
                "Confirm Email - Identity Manager",
                $"Please confirm your email by clicking the following link: {confirmationUrl}");

            return new EmailSenderServiceResult
            {
                Success = true,
                ErrorMessage = null,
                OperationDate = DateTime.Now,
                ConfirmationUrl = confirmationUrl,
                UrlToUndecode = confirmationUrl
            };

        }

        //Send URL to confirm password change
        public async Task<EmailSenderServiceResult> generateForgotPasswordConfirmationUrlAndSendToEmail(ForgotPasswordDto dto)
        {

            var user = await _userManager.FindByEmailAsync(dto.Email!);
            var userId = await _userManager.GetUserIdAsync(user!);

            if (user == null || userId == null)
            {

                return new EmailSenderServiceResult
                {
                    Success = false,
                    ErrorMessage = "User cannot be null",
                    OperationDate = DateTime.Now
                };

            }

            var confirmationToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var confirmationUrl = GenerateConfirmationUrl(userId, confirmationToken, "reset-password");
            Console.WriteLine($"[DEBUG] Confirm Forgot Password URL: {confirmationUrl}");

            await SendEmailAsync(dto.Email!,
                "Change your password - Identity Manager",
                $"Recover your password by clicking the following link: {confirmationUrl}");

            return new EmailSenderServiceResult
            {
                Success = true,
                ErrorMessage = null,
                OperationDate = DateTime.Now,
                ConfirmationUrl = confirmationUrl,
                UrlToUndecode = confirmationUrl
            };

        }

        //Method to generate a Base64 URL
        private string GenerateConfirmationUrl(string userId, string token, string urlEndpoint)
        {
            string baseUrl = "http://localhost:4200";

            // Base64 URL-safe - SIN caracteres problemáticos
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var safeToken = Convert.ToBase64String(tokenBytes)
                .TrimEnd('=')        // Quitar =
                .Replace('+', '-')   // Quitar +
                .Replace('/', '_');  // Quitar /

            return $"{baseUrl}/{urlEndpoint}?userId={userId}&code={safeToken}";
        }

    }

}

public class EmailSenderServiceResult
{

    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ConfirmationUrl { get; set; }
    public DateTime OperationDate { get; set; }
    public string? UrlToUndecode { get; set; }

}

public static class Base64UrlSafe
{
    public static string Encode(string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
    
public static string Decode(string encoded)
    {
        var base64 = encoded.Replace('-', '+').Replace('_', '/');
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        var bytes = Convert.FromBase64String(base64);
        return Encoding.UTF8.GetString(bytes);
    }
}