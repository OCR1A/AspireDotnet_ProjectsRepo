using System.ComponentModel.DataAnnotations;

namespace IdentityManager.DTOs.AuthenticationDTOs
{

    public class RegisterDto
    {

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "{0} must contain between {2} - {1} characters.")]
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "{0} and {1} must match.")]
        public string? ConfirmPassword { get; set; }

    }

}