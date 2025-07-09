using Microsoft.EntityFrameworkCore;
using ProductsWebApi.Models;

namespace ProductsWebApi.Data
{

    public class ProductsWebApiDbContext : DbContext
    {

        public ProductsWebApiDbContext(DbContextOptions<ProductsWebApiDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            //Product table setup
            modelBuilder.Entity<Product>(entity =>
            {

                //Product primary key setup
                entity.HasKey(p => p.Id);

                //Product Id setup
                entity.Property(p => p.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INT");

                //Product Name setup
                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasColumnType("VARCHAR(60)");

                //Product Description setup
                entity.Property(p => p.Description)
                    .IsRequired()
                    .HasColumnType("VARCHAR(255)");

                //Product Brand setup
                entity.Property(p => p.Brand)
                    .IsRequired()
                    .HasColumnType("VARCHAR(60)");

                //Product Price setup
                entity.Property(p => p.Price)
                    .IsRequired()
                    .HasColumnType("NUMERIC(10,3)");

                //Product Stock setup
                entity.Property(p => p.Stock)
                    .IsRequired()
                    .HasColumnType("INT");

                //Product CategoryId setup
                entity.Property(p => p.CategoryId)
                    .IsRequired()
                    .HasColumnType("INT");

                //Foreign Key
                entity.HasOne(p => p.Category)
                    .WithMany()
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            //Categories table setup
            modelBuilder.Entity<Category>(entity =>
            {

                //Category primary key
                entity.HasKey(c => c.Id);

                //Category Id setup
                entity.Property(c => c.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INT");

                //Category Name setup
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasColumnType("VARCHAR(60)");

                //Category Description setup
                entity.Property(c => c.Description)
                    .IsRequired()
                    .HasColumnType("VARCHAR(255)");

                entity.Property(c => c.FatherCategoryId)
                    .IsRequired(false)
                    .HasColumnType("INT");

            });

        }

    }

}