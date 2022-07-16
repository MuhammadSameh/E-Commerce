using API.DTOs;
using AutoMapper;
using Core.Entities;
using System.Linq;

namespace API.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Inventory, InventoryToRepresent>()

                .ForMember(m => m.Brand, x => x.MapFrom(i => i.Product.Brand.Name))
                .ForMember(m => m.Category, x => x.MapFrom(i => i.Product.Category.Name))
                .ForMember(
                m => m.Medias,
                x => x.MapFrom(i => i.Product.Medias.Select(x=>x.PicUrl)) // Not Completed !! trying to fix
                )
                ;
        }

    }
}