using System.ComponentModel.DataAnnotations.Schema;

namespace Koi.BusinessObjects
{
    public class KoiFish : BaseEntity
    {
        public string Name { get; set; }
        public string? Origin { get; set; }
        public bool? Gender { get; set; }
        public DateTime Dob { get; set; }
        public int Length { get; set; }
        public int Weight { get; set; }
        public string? PersonalityTraits { get; set; }
        public int? DailyFeedAmount { get; set; }
        public DateTime? LastHealthCheck { get; set; }
        public bool? IsAvailableForSale { get; set; }
        public long Price { get; set; }
        public bool? IsConsigned { get; set; } = false;
        public int? OwnerId { get; set; }

        //navigation
        [ForeignKey("OwnerId")]
        public virtual User? Owner { get; set; }

        // Navigation property for many-to-many relationship with KoiBreed
        //public virtual ICollection<KoiFishKoiBreed> KoiFishKoiBreeds { get; set; }
        public virtual ICollection<KoiBreed> KoiBreeds { get; set; }

        public virtual ICollection<KoiDiary> KoiDiaries { get; set; }

        public virtual ICollection<KoiCertificate>? KoiCertificates { get; set; }
        public virtual ICollection<KoiFishImage>? KoiFishImages { get; set; }
        public virtual ICollection<ConsignmentForNurture>? ConsignmentForNurtures { get; set; }
    }
}