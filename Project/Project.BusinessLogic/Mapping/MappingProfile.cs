using AutoMapper;
using Project.BusinessLogic.EntitiesDTO;
using Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.BusinessLogic.Mapping {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<Category, CategoryDTO>()
                .ReverseMap();
        }
    }
}
