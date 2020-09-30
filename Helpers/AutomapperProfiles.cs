using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Extensions;
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
                        opt.MapFrom(src => src.DateOfBirth.CalculateAge()
                        )
                    );
            
            CreateMap<Photo, PhotoForUserDto>();
        }
    }
}