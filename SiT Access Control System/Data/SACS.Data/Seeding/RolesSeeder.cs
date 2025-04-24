using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SACS.Common;
using SACS.Data.Models;

namespace SACS.Data.Seeding
{
    /// <summary>
    /// Seeds required roles and makes sure every account
    /// has the default "User" role unless already in another role.
    /// </summary>
    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 1. Ensure both roles exist
            await EnsureRoleExistsAsync(roleManager, GlobalConstants.AdministratorRoleName); // "Admin"
            await EnsureRoleExistsAsync(roleManager, GlobalConstants.UserRoleName);          // "User"

            // 2. Add "User" role to everyone who currently has no roles
            var allUsers = await userManager.Users.ToListAsync();

            foreach (var user in allUsers)
            {
                var roles = await userManager.GetRolesAsync(user);

                if (roles.Count == 0) // user is role‑less
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.UserRoleName);
                }
            }
        }

        private static async Task EnsureRoleExistsAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                return;
            }

            var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to create role '{roleName}': " +
                    string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }
        }
    }
}
