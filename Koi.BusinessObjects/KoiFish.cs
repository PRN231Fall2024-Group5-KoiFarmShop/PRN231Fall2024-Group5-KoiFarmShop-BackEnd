using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.BusinessObjects
{
    public class KoiFish : BaseEntity
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
        public bool? IsAvailableForSale { get; set; }
        public long Price { get; set; }
        public bool? IsConsigned { get; set; }
        public bool? IsSold { get; set; }

        //foreign key
        public int? ConsignedBy { get; set; }

        //navigation
        [ForeignKey("ConsignedBy")]
        public virtual User? Consigner { get; set; }

        // Navigation property for many-to-many relationship with KoiBreed
        public virtual ICollection<KoiFishKoiBreed> KoiFishKoiBreeds { get; set; }

        public virtual ICollection<KoiCertificate>? KoiCertificates { get; set; }
    }
}