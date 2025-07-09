using IdentityManager.DTOs.AuthenticationDTOs;
using IdentityManager.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityManager.Services
{

    public class AuthenticationService
    {

        //Dependency Injection
        private readonly JwtService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationService(JwtService jwtService, UserManager<ApplicationUser> userManager)
        {
            _jwtService = jwtService;
            _userManager = userManager;
        }

        //Register new user
        public async Task<AuthenticationServiceResult> Register(RegisterDto dto)
        {

            //Validate not null entries
            if (dto.Email == null || dto.Name == null || dto.Password == null || dto.ConfirmPassword == null)
            {

                return new AuthenticationServiceResult
                {
                    JwtToken = null,
                    Success = false,
                    ErrorMessage = "Email, Name, Password and ConfirmPassword cannot be empty.",
                    OperationDate = DateTime.Now
                };

            }

            ApplicationUser newUser = new ApplicationUser()
            {

                UserName = dto.Email,
                Email = dto.Email,
                DateCreated = DateTime.Today,

            };

            var result = await _userManager.CreateAsync(newUser, dto.Password);

            if (result.Succeeded)
            {
                var jwtToken = _jwtService.GenerateJwtToken(dto.Email);
                return new()
                {
                    JwtToken = jwtToken,
                    Success = true,
                    ErrorMessage = null,
                    OperationDate = DateTime.Now
                };
            }


            string fullError = "";

            foreach (var error in result.Errors)
            {

                string currentError = Convert.ToString(error.Description);
                fullError = currentError;

            }

            return new()
            {
                JwtToken = null,
                Success = false,
                ErrorMessage = fullError,
                OperationDate = DateTime.Now
            };

        }

        //Sign in with already registered user
        public async Task<AuthenticationServiceResult> SignIn(SignInDto dto)
        {

            if (dto.Email == null || dto.Password == null)
            {
                return new AuthenticationServiceResult()
                {
                    JwtToken = null,
                    Success = false,
                    ErrorMessage = "Email and Password fields cannot be empty.",
                    OperationDate = DateTime.Now
                };
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user != null)
            {

                var rightCreddentials = await _userManager.CheckPasswordAsync(user, dto.Password);

                if (rightCreddentials)
                {
                    var jwtToken = _jwtService.GenerateJwtToken(dto.Email);
                    return new AuthenticationServiceResult()
                    {
                        JwtToken = jwtToken,
                        Success = true,
                        ErrorMessage = null,
                        OperationDate = DateTime.Now
                    };
                }

                return new AuthenticationServiceResult()
                {
                    JwtToken = null,
                    Success = false,
                    ErrorMessage = "Incorrect User or Password",
                    OperationDate = DateTime.Now
                };

            }

            return new AuthenticationServiceResult()
            {
                JwtToken = null,
                Success = false,
                ErrorMessage = "Incorrect User or Password",
                OperationDate = DateTime.Now
            };

        }
        

    }

}

public class AuthenticationServiceResult
{

    public string? JwtToken { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime OperationDate { get; set; }

}

//eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0M0BnbWFpbC5jb20iLCJqdGkiOiI4OTUzNzQxYy1mZTE3LTQyZGEtYTM2NS04YThjNmY1YmY3YjMiLCJleHAiOjE3NTE2NTIxMDYsImlzcyI6ImxvY2FsaG9zdDo1MDEwIiwiYXVkIjoibG9jYWxob3N0OjUwMTAifQ.9eYvtjURC2RMVbTOPuau1CRNJR1t4n5g7ojXb9yVqBA
//eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0M0BnbWFpbC5jb20iLCJqdGkiOiIxYzZlNGVlMi03NWQ3LTQxYTEtOTJiMy05YjgwMDhjNDQ2OTUiLCJleHAiOjE3NTE2NTIxMjcsImlzcyI6ImxvY2FsaG9zdDo1MDEwIiwiYXVkIjoibG9jYWxob3N0OjUwMTAifQ.LnYONevNKc02IINolD5LpLCEpqlFZ5W1xmjfx1ltQyQ