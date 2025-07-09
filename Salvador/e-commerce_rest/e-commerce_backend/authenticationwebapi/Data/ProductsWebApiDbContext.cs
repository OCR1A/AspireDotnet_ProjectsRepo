using IdentityManager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProductsWebApi.Data
{

    public class ProductsWebApiDbContext : IdentityDbContext<ApplicationUser>
    {

        public ProductsWebApiDbContext(DbContextOptions<ProductsWebApiDbContext> options) : base(options)
        {
        }

    }

}