using AutoMapper;
using Koi.Repositories.Entities;
using Koi.Repositories.Models.UserModels;

namespace Koi.Services.Mapper
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile()
        {
            CreateMap<User, UserDetailsModel>()
          // .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender != null && src.Gender == true ? "Male" : "Female"))
          .ReverseMap();

            CreateMap<UserUpdateModel, User>()
           //   .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToLower() == "male")).ReverseMap()
           .ReverseMap();

            CreateMap<User, UserDTO>()
          //   .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender != null && src.Gender == true ? "Male" : "Female"))
          .ReverseMap();
        }
    }
}