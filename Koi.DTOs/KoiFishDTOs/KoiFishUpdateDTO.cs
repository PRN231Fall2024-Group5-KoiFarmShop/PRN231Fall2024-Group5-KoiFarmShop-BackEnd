namespace Koi.DTOs.KoiFishDTOs
{
    public class KoiFishUpdateDTO
    {
        public string Name { get; set; }
        public string? Origin { get; set; }
        public bool? Gender { get; set; }
        public DateTime Dob { get; set; }
        public int Length { get; set; }
        public int Weight { get; set; }
        public bool? IsAvailableForSale { get; set; }
        public long Price { get; set; }
        public bool? IsSold { get; set; }
        public bool? IsDeleted { get; set; }
        public string? PersonalityTraits { get; set; }
        public int? DailyFeedAmount { get; set; }
        public DateTime? LastHealthCheck { get; set; }
        public List<int> KoiBreedIds { get; set; }
        public List<string> ImageUrls { get; set; }
    }

    public class KoiFishUpdateConsignerDTOs
    {
        public int ConsignedBy { get; set; }
    }
}