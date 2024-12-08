namespace API.AutoMapper
{
    using AutoMapper;
    using global::AutoMapper;
    using API.Models;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, Product>();
            CreateMap<Shop, Shop>();
        }
    }
}
