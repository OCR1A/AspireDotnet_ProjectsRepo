using System.ComponentModel.DataAnnotations;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace IdentityManager.Models
{

    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? Name { get; set; }
        public DateTime DateCreated { get; set; }
    }

}