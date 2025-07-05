using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IdentityManager.Models.ViewModels
{

    public class RegisterViewModel
    {

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 5)]
        public string? Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The {0} must be at least {2} characters long.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "{0} and {1} must match.")]
        public string? ConfirmPassword { get; set; }

    }

}