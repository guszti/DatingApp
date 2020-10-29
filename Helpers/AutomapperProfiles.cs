using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Extensions;
using DatingApp.API.Model;
using Microsoft.AspNetCore.JsonPatch;

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
                        opt.MapFrom(src => src.DateOfBirth.CalculateAge()
                        )
                    );
            
            CreateMap<Photo, PhotoForUserDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserUpdateDto>();
        }
    }
}