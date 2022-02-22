using System.Collections.Generic;
using AppleStore.Contracts.InputModels;
using AppleStore.Models;
using AppleStore.ViewModels;

namespace AppleStore.Contracts.Services
{
    public interface IAppleData
    {
        IEnumerable<Apple> GetAll();

        Apple Get(string id);

        int GetCount();

        void Add(IAppleInputModel model);

        void Edit(IAppleInputModel model);

        ICollection<DiscountsViewModel> GetPossibleDiscounts(string id);
    }
}
