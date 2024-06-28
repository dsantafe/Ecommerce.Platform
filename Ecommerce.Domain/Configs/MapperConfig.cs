using AutoMapper;
using Ecommerce.Domain.DTOs;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Configs
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
        }
    }
}
