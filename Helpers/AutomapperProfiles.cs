using System;
using System.Linq;
using AutoMapper;
using DatingApp.API.Extensions;
using DatingApp.API.Dtos;
using DatingApp.API.Model;

namespace DatingApp.API.Helpers
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<User, UserWithPhotosDto>()
                .ForMember(
                    dest => dest.MainPhotoUrl,
                    opt => opt.MapFrom(
                        src => src.Photos.FirstOrDefault(
                            o => o.IsMain
                        ).Url
                    )
                )
                .ForMember(dest => dest.Age,
                    opt =>
                        opt.MapFrom(
                            src => src.DateOfBirth.CalculateAge()
                        )
                );

            CreateMap<Photo, PhotoForUserDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserUpdateDto>();
            CreateMap<UserForRegisterDto, User>();

            CreateMap<Message, MessageDto>()
                .ForMember(
                    dest => dest.SourceUsername,
                    opt => opt.MapFrom(
                        src => src.Source.UserName
                    )
                )
                .ForMember(
                    dest => dest.TargetUsername,
                    opt => opt.MapFrom(
                        src => src.Target.UserName)
                )
                .ForMember(
                    dest => dest.SourcePhotoUrl,
                    opt => opt.MapFrom(
                        src => src.Source.Photos.FirstOrDefault(
                            p => p.IsMain
                        ).Url
                    )
                )
                .ForMember(
                    dest => dest.TargetPhotoUrl,
                    opt => opt.MapFrom(
                        src => src.Target.Photos.FirstOrDefault(
                            p => p.IsMain
                        ).Url
                    )
                );

            /*  Not needed since stolen class
            CreateMap<DateTime, DateTime>().ConvertUsing(
                d => DateTime.SpecifyKind(d, DateTimeKind.Utc)
            );
            */
        }
    }
}