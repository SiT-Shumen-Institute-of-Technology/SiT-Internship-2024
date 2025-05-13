using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SACS.Data.Models;

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
            const string firstName = "Adam";
            const string lastName = "Stiuinger";

            // Ensure the "Administrator" role exists
            if (!await roleManager.RoleExistsAsync(adminRoleName))
            {
                var roleResult = await roleManager.CreateAsync(new ApplicationRole(adminRoleName));
                if (!roleResult.Succeeded)
                {
                    throw new Exception($"Failed to create {adminRoleName} role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }
            }

            // Check if the admin user already exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                // Generate the username from the First and Last name
                var userName = GenerateUserName(firstName, lastName);

                adminUser = new ApplicationUser
                {
                    UserName = userName,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    LastName = lastName,
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create admin user: {errors}");
                }
            }

            // Assign the "Administrator" role if not already assigned
            if (!await userManager.IsInRoleAsync(adminUser, adminRoleName))
            {
                var addToRoleResult = await userManager.AddToRoleAsync(adminUser, adminRoleName);
                if (!addToRoleResult.Succeeded)
                {
                    throw new Exception($"Failed to add user {adminEmail} to {adminRoleName} role: {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
                }
            }
        }

        // Add this method inside the class, outside SeedAdminAsync
        private static string GenerateUserName(string firstName, string lastName)
        {
            // Replace with a space if you insist, but it's safer to use no space or a dot
            return $"{firstName.ToLower()}.{lastName.ToLower()}"; // e.g., adam.stiuinger
        }
    }
}
