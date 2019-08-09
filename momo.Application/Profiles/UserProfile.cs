using AutoMapper;
using momo.Application.Authorization.Secret.Dto;
using momo.Entity.Premission;
using System;
using System.Collections.Generic;
using System.Text;

namespace momo.Application.Profiles
{
   public  class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<IdentityUser, UserDto>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest=>dest.Role,opt=>opt.MapFrom(src=>src.Salt))
                .ForMember(dest=>dest.Email,opt=>opt.MapFrom(src=>src.Account))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<UserDto, IdentityUser>().ForMember(dest => dest.Name,opt=>opt.MapFrom(src=>src.UserName)) ;
        }
    }
}
