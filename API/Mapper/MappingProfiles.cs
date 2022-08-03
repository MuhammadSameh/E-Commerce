using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderRelatedEntities;
using System.Linq;

namespace API.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Inventory, InventoryDto>()
                .ForMember(m => m.Medias,
                x => x.MapFrom(i => i.Medias.Select(x => x.PicUrl))
                );

            CreateMap<Brand, BrandDto>();

            CreateMap<Product, ProductDto>()
                .ForMember(pdto => pdto.Brand, p => p.MapFrom(i => i.Brand.Name))
                .ForMember(pdto => pdto.Category, p => p.MapFrom(i => i.Category.Name));

            CreateMap<InventoryDto, Inventory>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dto => dto.Inventory, c => c.MapFrom(i => i.Inventory.Product.Name));
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, orderItemDto>();
        }

    }
}