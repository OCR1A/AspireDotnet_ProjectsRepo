using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ApplicationUserNamespace;
using Microsoft.EntityFrameworkCore;
using ApplicationUserNamespace.Model;

namespace IdentityManager.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

    }

}