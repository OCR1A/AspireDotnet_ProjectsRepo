using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IdentityManager;
using IdentityManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace IdentityManager.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Custom entities
        //public DbSet<ApplicationUser> MyProperty { get; set; }

    }

}