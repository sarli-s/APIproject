using AutoMapper;
using DTOs;
using Entitys;

namespace WebAPIShop
{
    public class AutoMapping: Profile
    {
        public AutoMapping() 
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, LoginUserDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderDTO, Order>();//.ForMember(d;
            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>();
        }
    }
}
