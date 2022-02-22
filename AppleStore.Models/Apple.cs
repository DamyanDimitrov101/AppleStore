using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AppleStore.Enums;
using AppleStore.Models.BaseModels;
using static AppleStore.Common.GlobalConstants.AppleFormConstants;

namespace AppleStore.Models
{
    public class Apple : BaseModel<string>
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public AppleType Type { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<PurchasedApples> PurchasedApples { get; set; }
    }
}
