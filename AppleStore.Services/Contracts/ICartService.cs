using AppleStore.InputModels;

namespace AppleStore.Services.Contracts
{
    public interface ICartService
    {
        void Create(string userId);
        void AddApple(AddAppleInputModel model);
        void Remove(string purchasedAppleId);
        void BuyPurchased(CartList_All_PurchasedInputModel model);

        CartList_All_PurchasedInputModel GetAllPurchased(string userId);
    }
}
