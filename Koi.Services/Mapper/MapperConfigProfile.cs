using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.ConsignmentDTOs;
using Koi.DTOs.DietDTOs;
using Koi.DTOs.KoiBreedDTOs;
using Koi.DTOs.KoiCertificateDTOs;
using Koi.DTOs.KoiDiaryDTOs;
using Koi.DTOs.KoiFishDTOs;
using Koi.DTOs.PaymentDTOs;
using Koi.DTOs.TransactionDTOs;
using Koi.DTOs.UserDTOs;
using Koi.DTOs.WalletDTOs;
using Koi.Repositories.Models.UserModels;

namespace Koi.Services.Mapper
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile()
        {
            CreateMap<DietCreateDTO, Diet>().ReverseMap();
            CreateMap<DietDTO, Diet>().ReverseMap();
            ///
            CreateMap<KoiFishDiaryCreateDTO, KoiDiary>().ReverseMap();
            CreateMap<KoiFishDiaryUpdateDTO, KoiDiary>().ReverseMap();
            CreateMap<KoiBreed, KoiBreedCreateDTO>().ReverseMap();
            CreateMap<KoiBreed, KoiBreedResponseDTO>().ReverseMap();
            CreateMap<KoiCertificate, KoiCertificateResponseDTO>().ReverseMap();
            CreateMap<KoiFishImageDTO, KoiFishImage>().ReverseMap();
            CreateMap<KoiFish, KoiFishCreateDTO>()
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
            CreateMap<User, CustomerProfileDTO>().ReverseMap();

            //ORDER & WALLET
            CreateMap<OrderDTO, Order>().ReverseMap();
            CreateMap<WalletDTO, Wallet>().ReverseMap();
            CreateMap<TransactionDTO, Transaction>().ReverseMap();
            CreateMap<WalletTransactionDTO, WalletTransaction>().ReverseMap();

            //Consignment
            CreateMap<ConsignmentForNurtureDetailDTO, ConsignmentForNurture>().ReverseMap();
            CreateMap<ConsignmentForNurtureDTO, ConsignmentForNurture>().ReverseMap();
            CreateMap<ConsignmentRequestDTO, ConsignmentForNurture>().ReverseMap();
            CreateMap<ConsignmentUpdateDTO, ConsignmentForNurture>().ReverseMap();
        }
    }
}