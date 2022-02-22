using System;
using System.Collections.Generic;
using System.Linq;

using AppleStore.Contracts.InputModels;
using AppleStore.Contracts.Services;
using AppleStore.Enums;
using AppleStore.Models;
using AppleStore.ViewModels;

namespace AppleStore.Services
{
    public class InMemoryAppleData : IAppleData
    {
        private List<Apple> apples;
        private readonly List<Discount> discounts;

        public InMemoryAppleData()
        {
            this.apples = new List<Apple>()
            {
                new Apple(){
                    Id = "1",
                    Name = "Green",
                    Type = AppleType.Bitter,
                    Price = 600,
                    ImageUrl = "https://shop.widam.com.qa/images/detailed/2/apple-green-fragrance.jpg",
                    Description = "Green apples contain a compound called pectin, a fiber source that works as a prebiotic to promote the growth of healthy bacteria. The pectin found in green apples can help you break down foods more efficiently. The high fiber content in green apples can have other impacts on your digestive health as well."
                },
                new Apple(){
                    Id = "2",
                    Name = "Red",
                    Type = AppleType.Sweet,
                    Price = 400,
                    ImageUrl = "https://cdn.shopify.com/s/files/1/0101/0512/products/shutterstock_226100671_900x.jpg?v=1534100542",
                    Description = "Red Delicious apples have a very mild and sweet flavor. Most people find them bland in comparison to varieties such as Gala, Golden Delicious, or Fuji. Even so, the bright red color and historical significance of the Red Delicious apple makes it an iconic variety."
                },
                new Apple(){
                    Id = "3",
                    Name = "Yellow",
                    Type = AppleType.Juicy,
                    Price = 800,
                    ImageUrl = "https://www.gardeningknowhow.com/wp-content/uploads/2018/06/yellow-apples-400x267.jpg",
                    Description = "Low in calories, Golden Delicious apples are a good source of soluble fiber, which has been proven to help lower cholesterol, control weight, and regulate blood sugar. They also contain vitamins A and C, as well as a trace amount of boron and potassium, most of which is located in the apple's skin.."
                }
            };

            this.discounts = new List<Discount>()
            {
                new Discount()
                {
                    Id = Guid.NewGuid().ToString(),
                    AppleId = "2",
                    NewPrice = 700,
                    CreatedOn = DateTime.UtcNow,
                    Pairs = 2
                },
            };
        }

        public IEnumerable<Apple> GetAll()
            => this.apples;

        public Apple Get(string id)
            => this.apples.FirstOrDefault(a => a.Id == id);

        public int GetCount() 
            => (this.GetAll()).Count();

        public void Add(IAppleInputModel model)
            => this.apples.Add(new Apple()
            {
                Name = model.Name,
                Type = model.Type,
                CreatedOn = DateTime.UtcNow,
                ImageUrl = model.ImageUrl,
                ModifiedOn = DateTime.UtcNow,
                Description = model.Description,
            });

        public void Edit(IAppleInputModel model)
        {
            var apple = Get(model.Id);

            if (apple is null)
                throw new KeyNotFoundException();

            apple.Name = model.Name;
            apple.Description = model.Description;
            apple.ImageUrl = model.ImageUrl;
            apple.Type = model.Type;
            apple.ModifiedOn = DateTime.UtcNow;
        }

        public ICollection<DiscountsViewModel> GetPossibleDiscounts(string id) 
            => this.discounts
                .Where(d => d.AppleId == id)
                .Select(d=> new DiscountsViewModel()
                {
                    AppleId = d.AppleId,
                    Pairs = d.Pairs,
                    Apple = new AppleViewModel()
                    {
                        Id = d.Apple.Id,
                        Name = d.Apple.Name,
                        Price = d.Apple.Price,
                        ImageUrl = d.Apple.ImageUrl,
                    },
                    NewPrice = d.NewPrice
                })
                .ToList();
    }
}