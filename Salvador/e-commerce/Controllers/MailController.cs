using Microsoft.AspNetCore.Mvc;
using IdentityManager.Services;

namespace MailExternalService.Controllers
{
    public class MailController : Controller
    {
        private readonly EmailSender _emailService;

        public MailController(EmailSender emailService)
        {
            _emailService = emailService;
        }

        [Route("/send-email")]
        public async Task<IActionResult> SendEmail()
        {
            await _emailService.SendEmailAsync("usuario@demo.com", "Test email", "This is an email test using Mailtrap.");
            return Content("Email sent. Review Mailtrap.");
        }
    }
}