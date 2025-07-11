using Account.DTOs.AccountDTOs;
using IdentityManager.DTOs.AuthenticationDTOs;
using IdentityManager.Models;
using IdentityManager.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace PasswordManager.Services
{

    public class PasswordManagerService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailSenderService _emailSenderService;

        public PasswordManagerService(
            UserManager<ApplicationUser> userManager,
            EmailSenderService emailSenderService
        )
        {
            _userManager = userManager;
            _emailSenderService = emailSenderService;
        }

        public async Task<PasswordManagerServiceResult> ForgotPasswordRoute(ForgotPasswordDto dto)
        {

            //Validate not null entries
            if (dto.Email == null)
            {

                return new PasswordManagerServiceResult
                {
                    Success = false,
                    ErrorMessage = "Email, Name, Password and ConfirmPassword cannot be empty.",
                    OperationDate = DateTime.Now
                };

            }

            var result = await _userManager.FindByEmailAsync(dto.Email);

            if (result != null)
            {
                var forgotPasswordConfirmationUrl = await _emailSenderService.generateForgotPasswordConfirmationUrlAndSendToEmail(dto);
                if (forgotPasswordConfirmationUrl.Success == true)
                {
                    return new PasswordManagerServiceResult
                    {
                        Success = true,
                        ErrorMessage = null,
                        OperationDate = DateTime.Now
                    };
                }

                return new PasswordManagerServiceResult
                {
                    Success = false,
                    ErrorMessage = "Unable to send forgot password confirmation email. id=1",
                    OperationDate = DateTime.Now
                };

            }

            return new()
            {
                Success = false,
                ErrorMessage = "Unable to send forgot password confirmation email. id=2",
                OperationDate = DateTime.Now
            };

        }

    }

}

public class PasswordManagerServiceResult
{

    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime OperationDate { get; set; }

}