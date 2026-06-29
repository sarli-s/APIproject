using AutoMapper;
using DTOs;
using Entities;

namespace WebAPIShop
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            // ── User ────────────────────────────────────────────────────────────
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, LoginUserDTO>().ReverseMap();
            CreateMap<UserWithPasswordDTO, User>().ReverseMap();

            // ── Category ────────────────────────────────────────────────────────
            CreateMap<Category, CategoryDTO>()
                .ConstructUsing(src => new CategoryDTO(src.CategoryName))
                .ReverseMap();

            // ── Product ─────────────────────────────────────────────────────────
            CreateMap<Product, ProductDTO>()
                .ForCtorParam("Colors", opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.Colors) || src.Colors == "[]"
                        ? new string[0]
                        : src.Colors.Split(',', StringSplitOptions.RemoveEmptyEntries)))
                .ForCtorParam("Category", opt => opt.MapFrom(src =>
                    src.Category != null ? new CategoryDTO(src.Category.CategoryName) : null));

            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.Colors, opt => opt.MapFrom(src =>
                    src.Colors != null && src.Colors.Length > 0
                        ? string.Join(",", src.Colors)
                        : ""))
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.Category,   opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore());

            // ── Order ───────────────────────────────────────────────────────────
            CreateMap<Order, OrderDTO>()
                .ForCtorParam("userId", opt => opt.MapFrom(src => src.UserId))
                .ReverseMap()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.userId));

            // ── OrderItem ───────────────────────────────────────────────────────
            CreateMap<OrderItem, OrderItemDTO>()
                .ForCtorParam("ProductName",   opt => opt.MapFrom(src => src.Product.ProductName))
                .ForCtorParam("Price",         opt => opt.MapFrom(src => src.Product.Price))
                .ForCtorParam("Popularcolore", opt => opt.MapFrom(src => src.Popularcolore))
                .ForCtorParam("Customtext",    opt => opt.MapFrom(src => src.Customtext))
                .ReverseMap()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Order,   opt => opt.Ignore());
        }
    }
}
