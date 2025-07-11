using System.Runtime.CompilerServices;
using IdentityManager.DTOs;
using IdentityManager.DTOs.AuthenticationDTOs;
using IdentityManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityManager.Services
{

    public class AuthenticationService
    {

        //Dependency Injection
        private readonly JwtService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailSenderService _emailSenderService;

        public AuthenticationService(JwtService jwtService,
            UserManager<ApplicationUser> userManager,
            EmailSenderService emailSenderService
        )
        {
            _jwtService = jwtService;
            _userManager = userManager;
            _emailSenderService = emailSenderService;
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
                Name = dto.Name,
                DateCreated = DateTime.Today

            };

            var result = await _userManager.CreateAsync(newUser, dto.Password);

            if (result.Succeeded)
            {
                var emailConfirmationUrl = await _emailSenderService.generateEmailConfirmationUrlAndSendToEmail(dto);
                if (emailConfirmationUrl.Success == true)
                {
                    return new AuthenticationServiceResult
                    {
                        JwtToken = null,
                        Success = true,
                        ErrorMessage = null,
                        OperationDate = DateTime.Now
                    };
                }

                return new AuthenticationServiceResult
                {
                    JwtToken = null,
                    Success = false,
                    ErrorMessage = "Unable to send confirmation email",
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
                var ConfirmedEmail = await _userManager.IsEmailConfirmedAsync(user);
                var rightCreddentials = await _userManager.CheckPasswordAsync(user, dto.Password);

                if (rightCreddentials && ConfirmedEmail)
                {
                    var jwtToken = _jwtService.GenerateJwtToken(dto.Email, true);
                    return new AuthenticationServiceResult()
                    {
                        JwtToken = jwtToken,
                        Success = true,
                        ErrorMessage = null,
                        OperationDate = DateTime.Now, 
                        EmailConfirmed = ConfirmedEmail
                    };
                }
                else if (!rightCreddentials)
                {
                    return new AuthenticationServiceResult()
                    {
                        JwtToken = null,
                        Success = false,
                        ErrorMessage = "Incorrect User or Password.",
                        OperationDate = DateTime.Now
                    };
                }
                else if (rightCreddentials && !ConfirmedEmail)
                {
                    var jwtToken = _jwtService.GenerateJwtToken(dto.Email, false);
                    return new AuthenticationServiceResult()
                    {
                        JwtToken = jwtToken,
                        Success = true,
                        ErrorMessage = "Email not confirmed yet.",
                        OperationDate = DateTime.Now,
                        EmailConfirmed = ConfirmedEmail
                    };
                }

            }

            return new AuthenticationServiceResult()
            {
                JwtToken = null,
                Success = false,
                ErrorMessage = "Incorrect User or Password",
                OperationDate = DateTime.Now
            };

        }

        //Method to confirm your email 
        public async Task<AuthenticationServiceResult> ConfirmEmail(ConfirmEmailDto dto)
        {

            if (dto.UserId == null || dto.ConfirmEmailToken == null)
            {
                return new AuthenticationServiceResult
                {
                    JwtToken = null,
                    Success = false,
                    ErrorMessage = "UserId and code cannot be null",
                    OperationDate = DateTime.Now,
                    Token = null,
                    UserId = null
                };
            }

            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
            {
                return new AuthenticationServiceResult
                {
                    JwtToken = null,
                    Success = false,
                    ErrorMessage = "User not found.",
                    OperationDate = DateTime.Now,
                    Token = null,
                    UserId = null
                };
            }

            var userName = user.Name != null ? user.Name : "Not Found";

            var undecodedCode = Base64UrlSafe.Decode(dto.ConfirmEmailToken);

            var result = await _userManager.ConfirmEmailAsync(user, undecodedCode);

            if (!result.Succeeded)
            {
                var newJwtToken = _jwtService.GenerateJwtToken(userName, true);
                return new AuthenticationServiceResult
                {
                    JwtToken = newJwtToken,
                    Success = false,
                    ErrorMessage = "Cannot confirm email.",
                    OperationDate = DateTime.Now,
                    Token = null,
                    UserId = null
                };
            }

            return new AuthenticationServiceResult
            {
                JwtToken = null,
                Success = true,
                ErrorMessage = null,
                OperationDate = DateTime.Now,
                Token = null,
                UserId = null,
                EmailConfirmed = true
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
    public string? Token { get; set; }
    public int? UserId { get; set; }
    public bool? EmailConfirmed { get; set; }

}

//eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0M0BnbWFpbC5jb20iLCJqdGkiOiI4OTUzNzQxYy1mZTE3LTQyZGEtYTM2NS04YThjNmY1YmY3YjMiLCJleHAiOjE3NTE2NTIxMDYsImlzcyI6ImxvY2FsaG9zdDo1MDEwIiwiYXVkIjoibG9jYWxob3N0OjUwMTAifQ.9eYvtjURC2RMVbTOPuau1CRNJR1t4n5g7ojXb9yVqBA
//eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0M0BnbWFpbC5jb20iLCJqdGkiOiIxYzZlNGVlMi03NWQ3LTQxYTEtOTJiMy05YjgwMDhjNDQ2OTUiLCJleHAiOjE3NTE2NTIxMjcsImlzcyI6ImxvY2FsaG9zdDo1MDEwIiwiYXVkIjoibG9jYWxob3N0OjUwMTAifQ.LnYONevNKc02IINolD5LpLCEpqlFZ5W1xmjfx1ltQyQ