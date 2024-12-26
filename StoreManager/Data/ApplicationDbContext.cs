using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StoreManager.Data.Models;

namespace StoreManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<IdentityUserRole<string>> UserRoles { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Drinks", Description = "Beverages" },
                new Category { CategoryId = 2, Name = "Snacks", Description = "Light meals and snacks" },
                new Category { CategoryId = 3, Name = "Fruits", Description = "Fresh fruits" },
                new Category { CategoryId = 4, Name = "Vegetables", Description = "Fresh vegetables" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Name = "Coke", Description = "Soft drink", Price = 1.5M, Stock = 100, CategoryId = 1 },
                new Product { ProductId = 2, Name = "Chips", Description = "Potato chips", Price = 2.0M, Stock = 50, CategoryId = 2 },
                new Product { ProductId = 3, Name = "Apple", Description = "Fresh apple", Price = 0.5M, Stock = 200, CategoryId = 3 },
                new Product { ProductId = 4, Name = "Carrot", Description = "Fresh carrot", Price = 0.3M, Stock = 150, CategoryId = 4 }
            );
        }
    }
}
