using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SACS.Data.Models;

namespace SACS.Data.Seeding
{
    public class DepartmentsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Departments.AnyAsync())
            {
                return; // Already seeded
            }

            var departments = new[]
            {
                new Department { Name = "Human Resources" },
                new Department { Name = "Finance" },
                new Department { Name = "IT" },
                new Department { Name = "Marketing" },
            };

            await dbContext.Departments.AddRangeAsync(departments);
            await dbContext.SaveChangesAsync();
        }
    }
}
