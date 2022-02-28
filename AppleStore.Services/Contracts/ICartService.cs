using AppleStore.InputModels;
using System.Collections.Generic;

namespace AppleStore.Services.Contracts
{
    public interface ICartService
    {
        void Create(string userId);
        void AddApple(AddAppleInputModel model);
        void Remove(string purchasedAppleId, bool isAdmin);
        void BuyPurchased(string cartId, ICollection<string> allPurchased);
        CartList_All_PurchasedInputModel GetAllPurchased(string userId);
    }
}
