﻿namespace Koi.BusinessObjects.DTO.KoiFishDTOs
{
    public class KoiFishResponseDTO
    {
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
        public Int64 Price { get; set; }
        public bool? IsConsigned { get; set; }
        public bool? IsSold { get; set; }
        public int ConsignedBy { get; set; }
    }
}
