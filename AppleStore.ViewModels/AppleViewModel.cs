using System.Collections.Generic;

namespace AppleStore.ViewModels
{
    public class AppleViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public string ImageUrl { get; set; }

        public string Type { get; set; }
        
        public decimal Price { get; set; }
        
        public string CurrentUserId { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }

        public ICollection<DiscountsViewModel> PossibleDiscounts { get; set; } = new List<DiscountsViewModel>();
    }
}