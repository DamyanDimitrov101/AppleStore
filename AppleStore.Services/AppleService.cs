using System.Collections.Generic;
using System.Linq;

using AppleStore.Contracts.InputModels;
using AppleStore.Contracts.Services;
using AppleStore.InputModels;
using AppleStore.Models;
using AppleStore.Models.Repositories;
using AppleStore.Services.AutoMapperConfig;
using AppleStore.Services.Contracts;
using AppleStore.ViewModels;

namespace AppleStore.Services
{
    public class AppleService : IAppleService
    {
        private readonly IAppleData _appleData;
        private readonly IRepository<PurchasedApples> _purchasedRepository;

        public AppleService(
            IAppleData appleData, 
            IRepository<PurchasedApples> purchasedRepository)
        {
            this._appleData = appleData;
            this._purchasedRepository = purchasedRepository;
        }

        public IEnumerable<AppleViewModel> GetAll()
        {
            var model = this._appleData.GetAll();
            if (!model.Any())
                throw new KeyNotFoundException(nameof(model));
            
            var appleViewModelList = model.MapTo<List<AppleViewModel>>();
            return appleViewModelList;
        }

        public AppleViewModel Get(string id, string userId)
        {
            var all =  this._appleData.GetAll();

            var model = this._appleData.Get(id);
            if (model is null)
                throw new KeyNotFoundException(nameof(model));

            var appleViewModel = model.MapTo<AppleViewModel>();
            appleViewModel.PossibleDiscounts = this._appleData.GetPossibleDiscounts(model.Id);
            appleViewModel.CurrentUserId = userId;
            return appleViewModel;
        }

        public IAppleInputModel GetAsForm(string id, string userId)
        {
            var model = this._appleData.Get(id);
            if (model is null)
                throw new KeyNotFoundException(nameof(model));

            var appleViewModel = model.MapTo<AppleInputModel>();     
            return appleViewModel;
        }


        public void Add(IAppleInputModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrEmpty(model.ImageUrl)) 
                return;
            this._appleData.Add(model);
        }

        public void Edit(IAppleInputModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Name) 
                && !string.IsNullOrEmpty(model.ImageUrl)) 
                this._appleData.Edit(model);
        }

        public ICollection<CartListPurchasedAppleFormModel> GetPurchased() 
            => this._purchasedRepository
                .AllAsNoTracking()
                .Where(p => p.IsPurchased)
                .MapTo<List<CartListPurchasedAppleFormModel>>();
    }
}
