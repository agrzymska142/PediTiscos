using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreManager.Data.Models;

namespace StoreManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data (optional for testing)
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Drinks", Description = "Beverages" },
                new Category { CategoryId = 2, Name = "Snacks", Description = "Light meals and snacks" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Name = "Coke", Description = "Soft drink", Price = 1.5M, Stock = 100, CategoryId = 1 },
                new Product { ProductId = 2, Name = "Chips", Description = "Potato chips", Price = 2.0M, Stock = 50, CategoryId = 2 }
            );
        }
    }
}