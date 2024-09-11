using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.BusinessObjects
{
    public class KoiCertificate : BaseEntity
    {
        public int KoiFishId { get; set; } // Foreign key
        public virtual KoiFish KoiFish { get; set; } // Navigation property

        public string? CertificateType { get; set; }

        public string? CertificateUrl { get; set; }
    }
}