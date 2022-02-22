using System.Collections.Generic;
using AppleStore.Contracts.InputModels;
using AppleStore.InputModels;
using AppleStore.ViewModels;

namespace AppleStore.Services.Contracts
{
    public interface IAppleService
    {
        IEnumerable<AppleViewModel> GetAll();

        AppleViewModel Get(string id, string userId);
        
        IAppleInputModel GetAsForm(string id, string userId);

        void Add(IAppleInputModel model);

        void Edit(IAppleInputModel model);
        
        ICollection<CartListPurchasedAppleFormModel> GetPurchased();
    }
}
