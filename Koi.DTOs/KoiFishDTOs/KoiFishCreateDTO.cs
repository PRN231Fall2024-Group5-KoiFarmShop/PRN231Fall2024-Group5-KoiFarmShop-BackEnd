namespace Koi.DTOs.KoiFishDTOs
{
    public class KoiFishCreateDTO
    {
        public string Name { get; set; }
        public string? Origin { get; set; }
        public string? Gender { get; set; }
        public DateTime Dob { get; set; }
        public int Length { get; set; }
        public int Weight { get; set; }
        public bool? IsAvailableForSale { get; set; }
        public long Price { get; set; }
        public bool? IsSold { get; set; }
        public string? PersonalityTraits { get; set; }
        public int? DailyFeedAmount { get; set; }
        public DateTime? LastHealthCheck { get; set; }
        public List<int> KoiBreedIds { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<KoiCertificateCreateInfo>? Certificates { get; set; }
    }

    public class KoiCertificateCreateInfo
    {
        public string CertificateUrl { get; set; }
        public string CertificateType { get; set; }
    }
}
