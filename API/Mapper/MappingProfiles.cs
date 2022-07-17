﻿using API.DTOs;
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
                .ForMember(m => m.Medias,
                x => x.MapFrom(i => i.Medias.Select(x => x.PicUrl)) 
                );

            CreateMap<Product, ProductDto>()
                .ForMember(pdto => pdto.Brand, p => p.MapFrom(i => i.Brand.Name))
                .ForMember(pdto => pdto.Category, p => p.MapFrom(i => i.Category.Name));
        }

    }
}