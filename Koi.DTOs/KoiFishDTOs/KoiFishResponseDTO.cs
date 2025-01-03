﻿using Koi.DTOs.KoiBreedDTOs;
using Koi.DTOs.KoiCertificateDTOs;
using Koi.DTOs.KoiDiaryDTOs;
using Koi.DTOs.UserDTOs;

namespace Koi.DTOs.KoiFishDTOs
{
    public class KoiFishResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Origin { get; set; }
        public string? Gender { get; set; }
        public DateTime Dob { get; set; }
        public int Length { get; set; }
        public int Weight { get; set; }
        public string? PersonalityTraits { get; set; }
        public int? DailyFeedAmount { get; set; }
        public DateTime? LastHealthCheck { get; set; }
        public bool? IsAvailableForSale { get; set; } = false;
        public long Price { get; set; }
        public bool? IsConsigned { get; set; } = false;
        public bool? IsSold { get; set; } = false;
        public int? OwnerId { get; set; }
        public List<KoiCertificateResponseDTO> KoiCertificates { get; set; }
        public List<KoiBreedResponseDTO> KoiBreeds { get; set; }
        public List<KoiFishImageDTO> KoiFishImages { get; set; }
        public List<KoiFishDiaryCreateDTO> KoiDiaries { get; set; }
        public CustomerProfileDTO Owner { get; set; }
    }
}
