using Koi.DTOs.KoiCertificateDTOs;
using Koi.Repositories.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Services.Interface
{
    public interface IKoiCertificateService
    {
        Task<bool> DeleteKoiCertificate(int id);
        Task<KoiCertificateResponseDTO> UpdateKoiCertificate(UpdateKoiCertificateDTO dto, int id);
        Task<KoiCertificateResponseDTO> CreateKoiCertificate(CreateKoiCertificateDTO dto);
        Task<List<KoiCertificateResponseDTO>> GetKoiCertificates(KoiCertificateParams certificateParams);
        Task<KoiCertificateResponseDTO> GetKoiCertificateById(int id);
        Task<List<KoiCertificateResponseDTO>> GetListCertificateByKoiId(int koiId);
    }
}
