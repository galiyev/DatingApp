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
            .ForMember(dest=>dest.PhotoUrl, 
                opt=>opt.MapFrom(src=>src.Photos.FirstOrDefault(x=>x.IsMain).Url))
            .ForMember(dest =>dest.Age,
                opt => opt.MapFrom(src=>src.DateOfBirth.CalculateAge()));

        CreateMap<MemberUpdateDto, AppUser>();

        CreateMap<Photo, PhotoDto>();
        
        CreateMap<RegisterDto, AppUser>();

        CreateMap<Message, MessageDto>()
            .ForMember(dest => dest.SenderPhotoUrl,
                opt => opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.RecipientPhotoUrl,
                opt => opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));

        CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue?DateTime.SpecifyKind(d.Value, DateTimeKind.Utc):null);

    }
}   