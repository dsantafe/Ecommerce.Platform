namespace Ecommerce.Application.Configs
{
    using AutoMapper;
    using Ecommerce.Application.DTOs;
    using Ecommerce.Domain.Entities;

    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
        }
    }
}
