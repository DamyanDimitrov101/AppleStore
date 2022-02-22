using AppleStore.Models.Context;
using System;

namespace AppleStore.Data.Seeders
{
    public interface ISeeder
    {
        void Seed(AppleStoreDbContext dbContext);
    }
}
