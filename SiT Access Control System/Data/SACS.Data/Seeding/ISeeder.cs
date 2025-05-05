using System;
using System.Threading.Tasks;

namespace SACS.Data.Seeding;

public interface ISeeder
{
    Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider);
}