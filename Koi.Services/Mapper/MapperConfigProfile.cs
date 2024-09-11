using AutoMapper;
using Koi.BusinessObjects.DTO.KoiBreedDTOs;
using Koi.BusinessObjects.DTO.KoiFishDTOs;
using Koi.Repositories.Entities;

using Koi.Repositories.Entities;

using Koi.Repositories.Models.UserModels;

namespace Koi.Services.Mapper
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile()
        {
            CreateMap<KoiBreed, CreateKoiBreedDTO>().ReverseMap();

            CreateMap<KoiBreed, KoiBreedResponseDTO>().ReverseMap();

            CreateMap<KoiFish, CreateKoiFishDTO>()
                .ForMember(dest => dest.Gender,
                    opt => opt.MapFrom(src => src.Gender != null && src.Gender == true ? "Male" : "Female"))
                .ReverseMap();

            CreateMap<KoiFish, KoiFishResponseDTO>()
                .ForMember(dest => dest.Gender,
                    opt => opt.MapFrom(src => src.Gender != null && src.Gender == true ? "Male" : "Female"))
                .ReverseMap();

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