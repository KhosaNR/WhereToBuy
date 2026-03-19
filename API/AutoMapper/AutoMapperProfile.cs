namespace API.AutoMapper
{
    using API.Models;
    using API.Models.Dtos;
    using API.Models.PriceModels;
    using global::AutoMapper;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<Shop, ShopDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<Price, PriceDto>().ReverseMap();
            CreateMap<PromotionPrice, PromotionPriceDto>().ReverseMap();

            CreateMap<StockList, StockListDto>().ReverseMap();
            CreateMap<StockListProduct, StockListProductDto>().ReverseMap();
        }
    }
}