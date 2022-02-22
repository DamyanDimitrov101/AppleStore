using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using AppleStore.Contracts.InputModels;
using AppleStore.Enums;

using static AppleStore.Common.GlobalConstants.AppleFormConstants;

namespace AppleStore.InputModels
{
    public class AppleInputModel : IAppleInputModel
    {
        public AppleInputModel()
        {
        }

        public AppleInputModel(string name, string imageUrl, string description, AppleType type, decimal price)
        {
            Name = name;
            ImageUrl = imageUrl;
            Description = description;
            Type = type;
            Price = price;
        }

        public string Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        [DisplayName("Name of the apple")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Image")]
        public string ImageUrl { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [DisplayName("Type of apple")]
        public AppleType Type { get; set; }

        public decimal Price { get; set; }
    }
}