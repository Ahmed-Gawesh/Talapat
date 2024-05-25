using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;


namespace Talabat.APIs.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(PD => PD.ProductBrand, O => O.MapFrom(P => P.ProductBrand))
                .ForMember(PD => PD.ProductType, O => O.MapFrom(P => P.ProductType))
                .ForMember(PD => PD.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());



            CreateMap<Talabat.Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<AddressDto, Talabat.Core.Entities.Order_Aggregation.Address>().ReverseMap();

        }
    }
}
