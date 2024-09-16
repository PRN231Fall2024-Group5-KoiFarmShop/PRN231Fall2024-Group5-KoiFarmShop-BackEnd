using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.KoiCertificateDTOs
{
    public class KoiCertificateResponseDTO
    {
        public int Id { get; set; }
        public int KoiFishId { get; set; }
        public string? CertificateType { get; set; }

        public string? CertificateUrl { get; set; }
    }
}
