using Microsoft.AspNetCore.Identity;

namespace IdentityManager.Models
{

    public class ApplicationUser : IdentityUser
    {

        public string? Name { get; set; }
        public DateTime DateCreated { get; set; }        

    }

}