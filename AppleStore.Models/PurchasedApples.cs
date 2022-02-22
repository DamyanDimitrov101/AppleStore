using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using AppleStore.Models.BaseModels;

namespace AppleStore.Models
{
    public class PurchasedApples : BaseModel<string>
    {
        public int Count { get; set; }

        [Required]
        public string AppleId { get; set; }

        [ForeignKey(nameof(AppleId))]
        public Apple Apple { get; set; }

        public bool IsPurchased { get; set; }

        [Required]
        public string CartId { get; set; }

        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; }
    }
}
