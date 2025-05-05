using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SACS.Data.Seeding
{
    namespace SACS.Data.Seeding
    {
        /// <summary>
        ///     Runs all seeders needed to bootstrap the database.
        /// </summary>
        public class ApplicationDbContextSeeder : ISeeder
        {
            public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
            {
                if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

                if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

                var logger = serviceProvider.GetRequiredService<ILoggerFactory>()
                    .CreateLogger(typeof(ApplicationDbContextSeeder));

                // Order matters ─ Roles first, then anything that depends on them
                var seeders = new List<ISeeder>
                {
                    new RolesSeeder(),
                    new DepartmentsSeeder()
                };

                foreach (var seeder in seeders)
                {
                    await seeder.SeedAsync(dbContext, serviceProvider);
                    await dbContext.SaveChangesAsync();
                    logger.LogInformation($"Seeder {seeder.GetType().Name} completed.");
                }
            }
        }
    }
}