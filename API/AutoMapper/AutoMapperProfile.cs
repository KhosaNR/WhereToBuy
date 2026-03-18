namespace API.AutoMapper
{
    using AutoMapper;
    using global::AutoMapper;
    using API.Models;
    using API.Models.Dtos;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, Product>();
            CreateMap<Shop, Shop>();

            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<Shop, ShopDto>().ReverseMap();
        }
    }
}
