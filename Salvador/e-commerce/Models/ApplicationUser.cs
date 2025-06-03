using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ApplicationUserNamespace.Model
{

    public class ApplicationUser : IdentityUser
    {

        //[Required]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "{0} must have {2} to {1} characters.")]
        public string? Name { get; set; }

        [Range(99.99, 599.99, ErrorMessage = "{0} must be between {1} and {2}" )]
        public double? Price { get; set; }

    }

}