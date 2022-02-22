using AppleStore.Models;

namespace AppleStore.ViewModels
{
    public class DiscountsViewModel
    {
        public int Pairs { get; set; }

        public decimal NewPrice { get; set; }

        public string AppleId { get; set; }

        public AppleViewModel Apple { get; set; }
    }
}
