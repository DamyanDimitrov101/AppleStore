using System.ComponentModel.DataAnnotations;
using AppleStore.Models.BaseModels;

namespace AppleStore.Models
{
    public class Discount : BaseModel<string>
    {
        [Required]
        public int Pairs { get; set; }

        [Required]
        public decimal NewPrice { get; set; }

        [Required]
        public string AppleId { get; set; }

        public Apple Apple { get; set; }
    }
}
