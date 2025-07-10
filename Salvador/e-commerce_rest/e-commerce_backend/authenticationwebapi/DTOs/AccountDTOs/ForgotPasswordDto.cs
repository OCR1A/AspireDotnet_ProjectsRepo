using System.ComponentModel.DataAnnotations;

namespace Account.DTOs.AccountDTOs
{

    public class ForgotPasswordDto
    {

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

    }

}