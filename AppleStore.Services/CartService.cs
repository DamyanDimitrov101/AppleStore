using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using AppleStore.InputModels;
using AppleStore.Models;
using AppleStore.Services.Contracts;
using AppleStore.ViewModels;
using AppleStore.Models.Repositories;
using AppleStore.Services.AutoMapperConfig;

namespace AppleStore.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<Apple> _appleRepository;
        private readonly IRepository<Discount> _discountRepository;
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<ApplicationUser> _usersRepository;
        private readonly IRepository<PurchasedApples> _purchasedRepository;

        public CartService(
            IRepository<Cart> cartRepository,
            IRepository<ApplicationUser> usersRepository,
            IRepository<PurchasedApples> purchasedRepository,
            IRepository<Apple> appleRepository, 
            IRepository<Discount> discountRepository)
        {
            _cartRepository = cartRepository;
            _usersRepository = usersRepository;
            _purchasedRepository = purchasedRepository;
            _appleRepository = appleRepository;
            _discountRepository = discountRepository;
        }

        public CartList_All_PurchasedInputModel GetAllPurchased(string userId)
        {
            var user = this._usersRepository.GetById(userId);
            ThrowExceptionIfNull(user);

            var cart = this._cartRepository.GetById(user.CartId);
            ThrowExceptionIfNull(cart);

            var model = this._purchasedRepository
                    .AllAsNoTracking()
                    .Where(pa => pa.CartId == cart.Id)
                    .Where(pa => !pa.IsCompleted)
                    .Include(pa => pa.Apple)
                    .ToList();

            var purchasedModels  = model.MapTo<List<CartListPurchasedAppleFormModel>>();                    
            var (total, discounts) = this.GetTotal(purchasedModels);
            var allPurchased = new CartList_All_PurchasedInputModel(purchasedModels, total, discounts, cart.Id);

            return allPurchased;
        }

        public void Create(string userId)
        {
            var user = this._usersRepository.GetById(userId);
            ThrowExceptionIfNull(user);

            var cart = new Cart()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                User = user,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                PurchasedApplesList = new List<PurchasedApples>()
            };

            this._cartRepository.Add(cart);
            this._cartRepository.SaveChanges();
            
                user.CartId = this._cartRepository
                    .AllAsNoTracking()
                    .FirstOrDefault(c=> c.UserId == userId)
                    .Id;
                this._usersRepository.SaveChanges();
            
        }

        public void Remove(string purchasedAppleId, bool isAdmin)
        {
            var purchasedApple = this._purchasedRepository.GetById(purchasedAppleId);
            ThrowExceptionIfNull(purchasedApple);

            if (isAdmin)
            {
                purchasedApple.IsCompleted = true;
            }
            else
            {
                this._purchasedRepository.Delete(purchasedApple);
            }

            this._purchasedRepository.SaveChanges();
        }

        public void AddApple(AddAppleInputModel model)
        {
            var apple = this._appleRepository.GetById(model.AppleId);
            ThrowExceptionIfNull(apple);

            var cart = this. _cartRepository
                .All()
                .FirstOrDefault(c=> c.UserId == model.UserId);
            ThrowExceptionIfNull(cart);

            var purchasedApple = new PurchasedApples()
            {
                Id = Guid.NewGuid().ToString(),
                AppleId = model.AppleId,
                Apple = apple,
                Count = model.Count,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            };

            cart.PurchasedApplesList.Add(purchasedApple);
            this._cartRepository.SaveChanges();
        }

        public void BuyPurchased(string cartId, ICollection<string> allPurchased)
        {
            var cart = this._cartRepository.GetById(cartId);
            ThrowExceptionIfNull(cart);

            foreach (var purchaseId in allPurchased)
            {
                var purchasedApple = this._purchasedRepository
                    .GetById(purchaseId);
                ThrowExceptionIfNull(purchasedApple);

                purchasedApple.IsPurchased = true;
            }
            
            this._purchasedRepository.SaveChanges();
        }

        public void ThrowExceptionIfNull(object entity)
        {
            if (entity is null)
                throw new KeyNotFoundException(nameof(entity.GetType));
        }

        private (decimal, ICollection<DiscountsViewModel>) GetTotal(
            List<CartListPurchasedAppleFormModel> cartListPurchasedAppleFormModels)
        {
            var list = cartListPurchasedAppleFormModels
                .Where(p => !p.IsPurchased)
                .ToList();

            var sum = list
                .Sum(purchase => purchase.Count * purchase.Apple.Price);

            var discounts = ApplyDiscounts(list, ref sum);

            return (sum, discounts);
        }

        private ICollection<DiscountsViewModel> ApplyDiscounts(
            List<CartListPurchasedAppleFormModel> cartListPurchasedAppleFormModels,
            ref decimal sum)
        {
            var discountList = new List<DiscountsViewModel>();
            var grouped = cartListPurchasedAppleFormModels
                .GroupBy(pa => pa.AppleId)
                .ToDictionary(pa => pa.Key, pa => pa.ToList());

            foreach (var purchaseList in grouped.Values)
            {
                var totalCount = purchaseList
                    .Sum(pa => pa.Count);

                var appleId = purchaseList[0].AppleId;

                var discountMatch = this._discountRepository
                    .AllAsNoTracking()
                    .Include(d => d.Apple)
                    ?.Where(d => d.Pairs <= totalCount)
                    .FirstOrDefault(d => d.AppleId == appleId);

                if (discountMatch is null)
                    continue;

                var priceToExtract = discountMatch.Pairs * purchaseList[0].Apple.Price;
                sum -= priceToExtract;
                sum += discountMatch.NewPrice;

                var model = discountMatch.MapTo<DiscountsViewModel>();

                discountList.Add(model);
            }

            return discountList;
        }
    }
}
