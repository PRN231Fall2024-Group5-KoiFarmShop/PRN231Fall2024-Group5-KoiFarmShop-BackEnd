namespace Koi.DTOs.KoiFishDTOs
{
    public class UpdateKoiFishDTO
    {
        public string Name { get; set; }
        public string? Origin { get; set; }
        public bool? Gender { get; set; }
        public int Age { get; set; }
        public int Length { get; set; }
        public int Weight { get; set; }
        public string? PersonalityTraits { get; set; }
        public int? DailyFeedAmount { get; set; }
        public DateTime? LastHealthCheck { get; set; }
        public List<int> KoiBreedIds { get; set; }
        public int? ConsignedBy { get; set; }
    }
}
