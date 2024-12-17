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
        }
    }

}
