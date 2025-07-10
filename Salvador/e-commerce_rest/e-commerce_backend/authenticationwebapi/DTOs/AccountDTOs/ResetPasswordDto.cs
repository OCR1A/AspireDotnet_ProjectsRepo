using System.ComponentModel.DataAnnotations;

namespace Account.DTOs.AccountDTOs
{

    public class ResetPasswordDto
    {

        [Required]
        public string? UserId { get; set; }

        [Required]
        public string? ResetPasswordToken { get; set; }

    }

}