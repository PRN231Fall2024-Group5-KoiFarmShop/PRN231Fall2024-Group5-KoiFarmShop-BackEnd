using Koi.DTOs.KoiBreedDTOs;

namespace Koi.DTOs.KoiFishDTOs
{
    public class KoiFishResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Origin { get; set; }
        public string? Gender { get; set; }
        public int Age { get; set; }
        public int Length { get; set; }
        public int Weight { get; set; }
        public string? PersonalityTraits { get; set; }
        public int? DailyFeedAmount { get; set; }
        public DateTime? LastHealthCheck { get; set; }
        public bool? IsAvailableForSale { get; set; }
        public long Price { get; set; }
        public bool? IsConsigned { get; set; }
        public bool? IsSold { get; set; }
        public int? ConsignedBy { get; set; }
        public List<KoiBreedResponseDTO> KoiBreeds { get; set; }
        public List<KoiFishImageDTO> KoiFishImages { get; set; }
        public List<KoiFishDiaryCreateDTO> KoiDiaries { get; set; }
    }
}
