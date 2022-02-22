using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using AppleStore.Models.Context;
using AppleStore.Models;

namespace AppleStore.Data.Seeders
{
    public class DiscountSeeder : ISeeder
    {
        public void Seed(AppleStoreDbContext dbContext)
        {
            var redApple = dbContext.Apples.FirstOrDefault(a=> a.Name== "Red Apple");
            var yellowApple = dbContext.Apples.FirstOrDefault(a=> a.Name== "Yellow Apple");

            if(redApple is null || yellowApple is null)
                return;

            var discounts = new List<Discount>()
            {
                new Discount()
                {
                    Id = Guid.NewGuid().ToString(),
                    Pairs = 2,
                    NewPrice = 800,
                    AppleId = redApple.Id,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new Discount()
                {
                    Id = Guid.NewGuid().ToString(),
                    Pairs = 3,
                    NewPrice = 1600,
                    AppleId = yellowApple.Id,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
            };

            foreach (Discount discount in discounts)
            {
                var dbDiscount = dbContext.Discounts.FirstOrDefault(d => d.Pairs == discount.Pairs);

                if (dbDiscount is null)
                {
                    dbContext.Discounts.AddOrUpdate(discount);
                }
            }

            dbContext.SaveChanges();
        }
    }
}
