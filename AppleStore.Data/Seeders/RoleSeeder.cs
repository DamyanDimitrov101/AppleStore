using System;
using System.Collections.Generic;
using System.Linq;

using AppleStore.Models;
using AppleStore.Models.Context;
using Microsoft.AspNet.Identity;

namespace AppleStore.Data.Seeders
{
    public class RoleSeeder : ISeeder
    {
        public void Seed(AppleStoreDbContext dbContext)
        {
            var adminRole = dbContext.Roles.FirstOrDefault(a => a.Name == "Admin");
            var userRole = dbContext.Roles.FirstOrDefault(a => a.Name == "User");

            if (adminRole != null && userRole != null)
                return;

            var roles = new List<ApplicationRole>()
            {
                new ApplicationRole()
                {
                    Name = "Admin",
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false,                    
                },
                new ApplicationRole()
                {
                    Name = "User",
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false,
                },
            };

            foreach (ApplicationRole role in roles)
            {       
                dbContext.Roles.Add(role);
            }

            dbContext.SaveChanges();
        }
    }
}
