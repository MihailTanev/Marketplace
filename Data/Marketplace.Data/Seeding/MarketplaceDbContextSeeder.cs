﻿namespace Marketplace.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class MarketplaceDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(MarketplaceDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(MarketplaceDbContextSeeder));

            var seeders = new List<ISeeder>
                          {
                              new RolesSeeder(),
                              new CategoriesSeeder(),
                              new SubcategoriesSeeder(),
                              new ConditionsSeeder(),
                              new PromotionsSeeder()
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }
        }
    }
}
