using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using AppleStore.Models.BaseModels;

namespace AppleStore.Models
{
    public class Cart : BaseModel<string>
    {
        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        public decimal Total { get; set; }

        public ICollection<PurchasedApples> PurchasedApplesList { get; set; } = new List<PurchasedApples>();
    }
}
