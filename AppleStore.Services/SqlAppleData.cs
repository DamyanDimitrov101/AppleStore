using System;
using System.Collections.Generic;
using System.Linq;

using AppleStore.Contracts.InputModels;
using AppleStore.Contracts.Services;
using AppleStore.Models.Repositories;
using AppleStore.Models;
using AppleStore.ViewModels;
using AppleStore.Services.AutoMapperConfig;

namespace AppleStore.Services
{
    public class SqlAppleData : IAppleData
    {
        private readonly IRepository<Apple> _appleRepository;
        private readonly IRepository<Discount> _discountRepository;

        public SqlAppleData(
            IRepository<Apple> appleRepository, 
            IRepository<Discount> discountRepository)
        {
            _appleRepository = appleRepository;
            _discountRepository = discountRepository;
        }

        public virtual IEnumerable<Apple> GetAll()
            =>  this._appleRepository.AllAsNoTracking().ToList();

        public virtual Apple Get(string id)
            => this._appleRepository.GetById(id);

        public virtual int GetCount() 
            => (this.GetAll()).Count();

        public virtual void Add(IAppleInputModel model)
        {
            var apple = new Apple()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Type = model.Type,
                Price = model.Price,
                CreatedOn = DateTime.UtcNow,
                ImageUrl = model.ImageUrl,
                ModifiedOn = DateTime.UtcNow,
                Description = model.Description,
            };

            this._appleRepository.Add(apple);
            this._appleRepository.SaveChanges();
        }

        public void Edit(IAppleInputModel model)
        {
            var entity = Get(model.Id);
            if(Modify(entity, model))
                this._appleRepository.SaveChanges();
        }

        public ICollection<DiscountsViewModel> GetPossibleDiscounts(string id)
        {
            var discounts = this._discountRepository
                           .AllAsNoTracking()
                           .Where(d => d.AppleId == id);

            var result = discounts
            .Select(d => new DiscountsViewModel()
            {
                AppleId = d.AppleId,
                Pairs = d.Pairs,
                NewPrice = d.NewPrice,
                Apple = new AppleViewModel()
                {
                    Id = d.Apple.Id,
                    Name = d.Apple.Name,
                    Price = d.Apple.Price,
                    ImageUrl = d.Apple.ImageUrl
                }
            })
            .ToList();

            return result;
        }

        protected virtual bool Modify(Apple entity, IAppleInputModel model)
        {
            try
            {
                if (entity is null)
                    throw new KeyNotFoundException();

                var appleUpdated = new Apple
                {
                    Id = entity.Id,
                    Name = model.Name,
                    ImageUrl = model.ImageUrl,
                    Price = model.Price,
                    Type = model.Type,
                    Description = model.Description
                };
                
                this._appleRepository.Update(appleUpdated);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
