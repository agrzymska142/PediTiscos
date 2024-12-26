using Microsoft.AspNetCore.Identity;
using StoreManager.Data.Models;

namespace StoreManager.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Define roles
            var roles = new[] { "Administrator", "Customer", "Employee" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Define test users
            var users = new[]
            {
                new { Name = "John", Surname = "Doe", Email = "admin@example.com", Password = "Admin123!", Role = "Administrator" },
                new { Name = "Kate", Surname = "Smith", Email = "customer@example.com", Password = "Customer123!", Role = "Customer" },
                new { Name = "Bob", Surname = "Ross", Email = "employee@example.com", Password = "Employee123!", Role = "Employee" }
            };

            foreach (var testUser in users)
            {
                var user = await userManager.FindByEmailAsync(testUser.Email);
                if (user == null)
                {
                    user = new ApplicationUser { Name = testUser.Name, Surname = testUser.Surname, UserName = testUser.Email, Email = testUser.Email, EmailConfirmed = true };
                    var result = await userManager.CreateAsync(user, testUser.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, testUser.Role);
                    }
                }
            }

            // Seed categories
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { CategoryId = 1, Name = "Drinks", Description = "Beverages" },
                    new Category { CategoryId = 2, Name = "Snacks", Description = "Light meals and snacks" },
                    new Category { CategoryId = 3, Name = "Fruits", Description = "Fresh fruits" },
                    new Category { CategoryId = 4, Name = "Vegetables", Description = "Fresh vegetables" }
                );
            }

            // Seed products
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product { ProductId = 1, Name = "Coke", Description = "Soft drink", Price = 1.5M, Stock = 100, CategoryId = 1 },
                    new Product { ProductId = 2, Name = "Chips", Description = "Potato chips", Price = 2.0M, Stock = 50, CategoryId = 2 },
                    new Product { ProductId = 3, Name = "Apple", Description = "Fresh apple", Price = 0.5M, Stock = 200, CategoryId = 3 },
                    new Product { ProductId = 4, Name = "Carrot", Description = "Fresh carrot", Price = 0.3M, Stock = 150, CategoryId = 4 }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
