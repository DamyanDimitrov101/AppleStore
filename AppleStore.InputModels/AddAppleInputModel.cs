using System.ComponentModel.DataAnnotations;

namespace AppleStore.InputModels
{
    public class AddAppleInputModel
    {
        public int Count { get; set; }

        [Key]
        public string UserId { get; set; }

        [Key]
        public string AppleId { get; set; }
    }
}
