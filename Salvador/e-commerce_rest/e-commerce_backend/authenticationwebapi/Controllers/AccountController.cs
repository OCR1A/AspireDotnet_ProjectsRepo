using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Account.DTOs.AccountDTOs;
using IdentityManager.Models;
using IdentityManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Account.Controllers
{

    [ApiController]
    [Route("/account")]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailSenderService _emailSenderService;

        public AccountController(UserManager<ApplicationUser> userManager,
            EmailSenderService emailSenderService
        )
        {
            _userManager = userManager;
            _emailSenderService = emailSenderService;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {

            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userName!);

            MeDto dto = new MeDto()
            {
                Name = user!.Name,
                DateCreated = user.DateCreated,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled

            };

            return Ok(dto);

        }

    }

}