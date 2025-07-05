using System.Threading.Tasks;
using IdentityManager.DTOs.AuthenticationDTOs;
using IdentityManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
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

    }

}   