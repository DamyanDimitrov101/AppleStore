using System.Collections.Generic;
using AppleStore.Models;

namespace AppleStore.ViewModels
{
    public class CartViewModel
    {
        public string UserId { get; set; }
        
        public ICollection<PurchasedApples> PurchasedApples { get; set; }

        public decimal Total { get; set; }
    }
}