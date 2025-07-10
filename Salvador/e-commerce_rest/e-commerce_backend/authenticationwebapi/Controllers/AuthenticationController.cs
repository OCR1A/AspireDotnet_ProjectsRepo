using System.Threading.Tasks;
using IdentityManager.DTOs;
using IdentityManager.DTOs.AuthenticationDTOs;
using IdentityManager.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Esf;

namespace IdentityManager.Controllers
{

    [ApiController]
    [Route("/authentication")]
    public class AuthenticationController : ControllerBase
    {

        private readonly AuthenticationService _authService;
        public AuthenticationController(AuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {

            var result = await _authService.Register(dto);
            return result.Success == true ? Ok(result) : BadRequest(result);

        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignInDto dto)
        {

            var result = await _authService.SignIn(dto);
            return result.Success == true ? Ok(result) : BadRequest(result);

        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmailRoute([FromBody] ConfirmEmailDto dto)
        {

            if (dto.ConfirmEmailToken == null || dto.UserId == null)
            {
                return BadRequest("UserId and code cannot be null.");
            }

            var result = await _authService.ConfirmEmail(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            Console.WriteLine($"[DEBUG] User email confirmed successfully!");
            return Ok(result);

        }

    }

}   