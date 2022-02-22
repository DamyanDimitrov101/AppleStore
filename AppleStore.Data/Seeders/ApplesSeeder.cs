using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

using AppleStore.Models.Context;
using AppleStore.Enums;
using AppleStore.Models;

namespace AppleStore.Data.Seeders
{
    public class ApplesSeeder : ISeeder
    {
        public void Seed(AppleStoreDbContext dbContext)
        {
            var apples = new List<Apple>()
            {
                new Apple()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Green Apple",
                    Type = AppleType.Bitter,
                    ImageUrl = "https://shop.widam.com.qa/images/detailed/2/apple-green-fragrance.jpg",
                    CreatedOn = DateTime.UtcNow,
                    Description = "Green apples contain a compound called pectin, a fiber source that works as a prebiotic to promote the growth of healthy bacteria. The pectin found in green apples can help you break down foods more efficiently. The high fiber content in green apples can have other impacts on your digestive health as well.",
                    Price = 650,
                },
                new Apple()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Red Apple",
                    Type = AppleType.Sweet,
                    ImageUrl = "https://thekitchencommunity.org/wp-content/uploads/2021/06/What-Are-The-Sweetest-Apples-1200x900.jpg",
                    CreatedOn = DateTime.UtcNow,
                    Description = "Red Delicious apples have a very mild and sweet flavor. Most people find them bland in comparison to varieties such as Gala, Golden Delicious, or Fuji. Even so, the bright red color and historical significance of the Red Delicious apple makes it an iconic variety.",
                    Price = 450,
                },
                new Apple()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Yellow Apple",
                    Type = AppleType.Juicy,
                    ImageUrl = "https://envato-shoebox-0.imgix.net/4db2/027a-82a4-4165-92c7-b75164a2cb1b/181802_0970.jpg?auto=compress%2Cformat&fit=max&mark=https%3A%2F%2Felements-assets.envato.com%2Fstatic%2Fwatermark2.png&markalign=center%2Cmiddle&markalpha=18&w=1600&s=220ac95867914cea4f19522ac513f8c2",
                    CreatedOn = DateTime.UtcNow,
                    Description = "Similar in taste and texture to Red Delicious, Golden or Yellow Delicious apples are yellow-gold in appearance and have the same mild to sweet taste.These are rich in photochemical and researches prove that daily consumption of apple helps fight chronic diseases. Its peel consists of dietary fiber, mainly insoluble fiber, which helps in reducing weight.",
                    Price = 1250,
                },
                new Apple()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Blue Apple",
                    Type = AppleType.Salty,
                    ImageUrl = "https://media.istockphoto.com/photos/juicy-apples-in-trendy-blue-color-picture-id1193358022?k=20&m=1193358022&s=170667a&w=0&h=KmWyTAiDgT0P4_8sKCI8aNAcYRYSEBdLBnCV9u7-Yq8=",
                    CreatedOn = DateTime.UtcNow,
                    Description = "Blue Pearmain apples are also known under the names Maine Blue Pear, Blue Pearamell, Painbear Bluemain, Blue Pomade, and Blue Pearamay, and are one of the most famous New England apple varieties.",
                    Price = 2200,
                },
                new Apple()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Purple Apple",
                    Type = AppleType.Sour,
                    ImageUrl = "https://i.pinimg.com/originals/c4/e0/14/c4e014c049efb152022fffe7c6022f9e.jpg",
                    CreatedOn = DateTime.UtcNow,
                    Description = "The Purple Sugar Apple is a tropical, or sometimes subtropical, compound fruit that is roughly the size of a baseball. It has a thick velvety rind composed of knobby segments that give it a scale-like appearance.",
                    Price = 1800,
                }
            };

            foreach (Apple apple in apples)
            {
                var dbApple = dbContext.Apples.FirstOrDefault(x => x.Name == apple.Name);

                if (dbApple is null)
                {
                    dbContext.Apples.AddOrUpdate(apple);
                }
            }

            dbContext.SaveChanges();
        }
    }
}
