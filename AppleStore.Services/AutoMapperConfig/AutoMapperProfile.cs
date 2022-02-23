using AppleStore.InputModels;
using AppleStore.Models;
using AppleStore.ViewModels;
using AutoMapper;

namespace AppleStore.Services.AutoMapperConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            ShouldMapProperty = arg => arg.GetMethod.IsPublic || arg.GetMethod.IsAssembly;

            CreateMap<Apple, AppleViewModel>()
                .ForMember(a=> a.CurrentUserId, opt=> opt.Ignore())
                .ForMember(a=> a.PossibleDiscounts, opt=> opt.Ignore());
            CreateMap<Apple, AppleInputModel>();

            CreateMap<PurchasedApples, CartListPurchasedAppleFormModel>()
                .ForMember(pa => pa.PurchasedAppleId, opt => opt.MapFrom(pe => pe.Id))
                .ForMember(a => a.IsPurchased, opt => opt.MapFrom(pe => pe.IsPurchased));

            CreateMap<Discount, DiscountsViewModel>()
                .ForMember(a=> a.Apple, opt=> opt.MapFrom<Apple>(a=> a.Apple));
        }
    }
}
