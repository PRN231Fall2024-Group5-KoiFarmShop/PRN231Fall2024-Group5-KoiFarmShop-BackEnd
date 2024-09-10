using AutoMapper;
using Koi.BusinessObjects.DTO.KoiBreedDTOs;
using Koi.BusinessObjects.DTO.KoiFishDTOs;
using Koi.Repositories.Entities;

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
        }
    }
}