using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace IdentityManager.Modeles.ViewModels
{

    public class RegisterViewModel
    {

        [Required]
        public string? Name { get; set; } 

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "{0} and {1} must match.")]
        public string? ConfirmPassword { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

    }

}