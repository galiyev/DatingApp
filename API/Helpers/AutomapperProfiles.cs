﻿using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutomapperProfiles:Profile
{
    public AutomapperProfiles()
    {
        CreateMap<AppUser, MemberDto>()
            .ForMember(dest=>dest.PhotoUrl, opt=>opt.MapFrom(src=>src.Photos.FirstOrDefault(x=>x.IsMain).Url))
            .ForMember(dest =>dest.Age, opt => opt.MapFrom(src=>src.DateOfBirth.CalculateAge()));

        CreateMap<MemberUpdateDto, AppUser>();

        CreateMap<Photo, PhotoDto>();
        
        CreateMap<RegisterDto, AppUser>();
    }
}   