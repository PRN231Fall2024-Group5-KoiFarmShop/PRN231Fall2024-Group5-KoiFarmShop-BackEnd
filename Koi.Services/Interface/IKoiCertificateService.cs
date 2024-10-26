using Koi.BusinessObjects;
using Koi.DTOs.KoiCertificateDTOs;
using Koi.Repositories.Helper;

namespace Koi.Services.Interface
{
    public interface IKoiCertificateService
    {
        Task<bool> DeleteKoiCertificate(int id);
        Task<List<KoiCertificateResponseDTO>> GetKoiCertificates(KoiCertificateParams certificateParams);
        Task<KoiCertificateResponseDTO> UpdateKoiCertificate(UpdateKoiCertificateDTO dto, int id);
        Task<KoiCertificateResponseDTO> CreateKoiCertificate(CreateKoiCertificateDTO dto);
        IQueryable<KoiCertificate> GetKoiCertificates();
        Task<KoiCertificateResponseDTO> GetKoiCertificateById(int id);
        Task<List<KoiCertificateResponseDTO>> GetListCertificateByKoiId(int koiId);
    }
}
