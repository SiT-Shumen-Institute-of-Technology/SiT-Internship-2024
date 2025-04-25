using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SACS.Data.Models;
using System;
using System.Threading.Tasks;

namespace SACS.Data.Seeding
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            const string adminEmail = "admin@sacs.com";
            const string adminPassword = "Admin123!"; // Change in production
            const string adminRoleName = "Administrator";

            // Ensure role exists
            if (!await roleManager.RoleExistsAsync(adminRoleName))
            {
                await roleManager.CreateAsync(new ApplicationRole(adminRoleName));
            }

            // Check if the admin user already exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors));
                }
            }

            // Assign role if not already assigned
            if (!await userManager.IsInRoleAsync(adminUser, adminRoleName))
            {
                await userManager.AddToRoleAsync(adminUser, adminRoleName);
            }
        }
    }
}
