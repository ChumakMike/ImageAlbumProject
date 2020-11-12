using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
            CreateMap<Image, ImageDTO>()
                .ReverseMap();
            CreateMap<Rating, RatingDTO>()
                .ReverseMap();
            CreateMap<IdentityRole, RoleDTO>()
                .ReverseMap();
            CreateMap<ApplicationUser, UserDTO>()
                .ReverseMap();
        }
    }
}
