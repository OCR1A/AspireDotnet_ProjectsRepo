using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IdentityManager.Models.ViewModels
{

    public class ForgotPasswordViewModel
    {

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

    }

}