using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.KoiBreedDTOs;
using Koi.DTOs.KoiFishDTOs;
using Koi.DTOs.PaymentDTOs;
using Koi.DTOs.WalletDTOs;
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
                .ReverseMap().ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == "Male" ? true : false));

            CreateMap<KoiFish, KoiFishResponseDTO>()
                .ForMember(dest => dest.Gender,
                    opt => opt.MapFrom(src => src.Gender != null && src.Gender == true ? "Male" : "Female"))
                .ReverseMap().ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == "Male" ? true : false));

            CreateMap<User, UserDetailsModel>()
          // .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender != null && src.Gender == true ? "Male" : "Female"))
          .ReverseMap();

            CreateMap<UserUpdateModel, User>()
           //   .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToLower() == "male")).ReverseMap()
           .ReverseMap();

            CreateMap<User, UserDTO>()
          //   .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender != null && src.Gender == true ? "Male" : "Female"))
          .ReverseMap();

            //ORDER & WALLET
            CreateMap<OrderDTO, Order>().ReverseMap();
            CreateMap<WalletDTO, Wallet>().ReverseMap();
            CreateMap<TransactionDTO, Transaction>().ReverseMap();
        }
    }
}