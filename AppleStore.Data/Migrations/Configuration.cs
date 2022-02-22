using AppleStore.Models.Context;
using AppleStore.Data.Seeders;

namespace AppleStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<AppleStoreDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AppleStoreDbContext context)
        {
            var seeder = new AppleStoreDbContextSeeder();
            seeder.Seed(context);
        }
    }
}
