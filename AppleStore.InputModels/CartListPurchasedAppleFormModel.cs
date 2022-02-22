using AppleStore.Contracts.InputModels;

namespace AppleStore.InputModels
{
    public class CartListPurchasedAppleFormModel
    {
        public int Count { get; set; }

        public string AppleId { get; set; }

        public bool IsPurchased { get; set; }

        public IAppleInputModel Apple { get; set; }

        public string PurchasedAppleId { get; set; }
    }
}
