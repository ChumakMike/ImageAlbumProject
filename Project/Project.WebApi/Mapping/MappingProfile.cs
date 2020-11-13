using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Project.BusinessLogic.EntitiesDTO;
using Project.WebApi.Models;

namespace Project.WebApi.Mapping {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<CategoryDTO, CategoryVM>()
                .ReverseMap();
            CreateMap<ImageDTO, ImageVM>()
                .ReverseMap();
            CreateMap<RatingDTO, RatingVM>()
                .ReverseMap();
            CreateMap<UserDTO, UserVM>()
                .ReverseMap();
            CreateMap<RoleDTO, RoleVM>()
                .ReverseMap();
            ///------------
            CreateMap<IdentityRole, RoleVM>()
                .ReverseMap();
        }
    }
}
