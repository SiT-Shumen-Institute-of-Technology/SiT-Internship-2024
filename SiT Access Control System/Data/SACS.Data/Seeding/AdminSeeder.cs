using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SACS.Data;
using SACS.Data.Models;

namespace SACS.Data.Seeding;

public static class AdminSeeder
{
    public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

        const string adminEmail = "admin@sacs.com";
        const string adminPassword = "Admin123!";
        const string adminRoleName = "Administrator";

        // Ensure the department exists
        var department = dbContext.Departments.FirstOrDefault();
        if (department == null)
        {
            department = new Department
            {
                Name = "Administration"
            };

            dbContext.Departments.Add(department);
            await dbContext.SaveChangesAsync();
        }

        // Ensure role exists
        if (!await roleManager.RoleExistsAsync(adminRoleName))
            await roleManager.CreateAsync(new ApplicationRole(adminRoleName));

        // Check if the admin user already exists
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                DepartmentId = department.Id,
                FirstName = "Admin",
                LastName = "User",
                CreatedOn = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (!result.Succeeded)
                throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        // Assign role if not already assigned
        if (!await userManager.IsInRoleAsync(adminUser, adminRoleName))
            await userManager.AddToRoleAsync(adminUser, adminRoleName);
    }
}
