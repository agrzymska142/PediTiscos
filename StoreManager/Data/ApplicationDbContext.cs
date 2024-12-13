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


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Drinks", Description = "Beverages" },
                new Category { CategoryId = 2, Name = "Snacks", Description = "Light meals and snacks" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Name = "Coke", Description = "Soft drink", Price = 1.5M, Stock = 100, CategoryId = 1 },
                new Product { ProductId = 2, Name = "Chips", Description = "Potato chips", Price = 2.0M, Stock = 50, CategoryId = 2 }
            );

            // Seed Orders
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    OrderId = 1,
                    OrderDate = DateTime.Now.AddDays(-1),
                    ClientName = "John Doe",
                    ClientEmail = "johndoe@example.com",
                    TotalAmount = 7.5M,
                    Status = "Pending"
                },
                new Order
                {
                    OrderId = 2,
                    OrderDate = DateTime.Now.AddDays(-2),
                    ClientName = "Jane Smith",
                    ClientEmail = "janesmith@example.com",
                    TotalAmount = 3.0M,
                    Status = "Confirmed"
                }
            );

            // Seed OrderDetails
            modelBuilder.Entity<OrderDetail>().HasData(
                new OrderDetail { OrderDetailId = 1, OrderId = 1, ProductId = 1, Quantity = 2, UnitPrice = 1.5M },
                new OrderDetail { OrderDetailId = 2, OrderId = 1, ProductId = 2, Quantity = 1, UnitPrice = 2.0M },
                new OrderDetail { OrderDetailId = 3, OrderId = 2, ProductId = 2, Quantity = 1, UnitPrice = 2.0M }
            );
        }
    }
}