using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace IdentityManager.Services
{
    public class EmailSender : IEmailSender
    {

        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var user = _configuration["MailSenderConfig:Username"];
            var password = _configuration["MailSenderConfig:Password"];
            var host = _configuration["MailSenderConfig:SmtpHost"];
            var smtp_port = _configuration["MailSenderConfig:SmtpPort"];
            var smtpPort = int.Parse(smtp_port);

            Console.WriteLine($"smtpPort: {smtpPort}");
            Console.WriteLine($"User: {user}");
            Console.WriteLine($"Password: {password}");
            Console.WriteLine($"Host: {host}");

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
    }
}
