using AutoMapper;
using DALayer.Entities;
using Microsoft.AspNetCore.Identity;
using PLayer.Models;
using System.Collections.Generic;

namespace PLayer.Mapper
{
    public class MapperProfile :Profile
    {
        public MapperProfile()
        {
            CreateMap<CategoryViewModel, Category>().ReverseMap();
            CreateMap<MenuItemViewModel, MenuItem>().ReverseMap();
            CreateMap<AppUser, UserViewModel>().ReverseMap();
            CreateMap<IdentityRole, RolesViewModel>().
                ForMember(d=>d.RoleName ,o=>o.MapFrom(s=>s.Name)).ReverseMap(); 
        }
    }
}
