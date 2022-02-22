using AppleStore.Enums;

namespace AppleStore.Contracts.InputModels
{
    public interface IAppleInputModel
    {
        string Id { get; set; }

        string Name { get; set; }

        string ImageUrl { get; set; }

        string Description { get; set; }

        AppleType Type { get; set; }

        decimal Price { get; set; }
    }
}
