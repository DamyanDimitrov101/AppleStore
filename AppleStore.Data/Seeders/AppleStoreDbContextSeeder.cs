using System;
using System.Collections.Generic;
using AppleStore.Models.Context;

namespace AppleStore.Data.Seeders
{
    public class AppleStoreDbContextSeeder : ISeeder
    {
        public void Seed(AppleStoreDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            var seeders = new List<ISeeder>
            {
                new ApplesSeeder(),
                new DiscountSeeder(),
                new RoleSeeder()
            };

            foreach (var seeder in seeders)
            {
                seeder.Seed(dbContext);
                dbContext.SaveChanges();
            }
        }
    }
}
