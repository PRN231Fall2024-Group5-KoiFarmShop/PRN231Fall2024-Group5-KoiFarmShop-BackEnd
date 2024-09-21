using Koi.BusinessObjects;
using Koi.DTOs.KoiFishDTOs;
using Koi.Repositories.Helper;

namespace Koi.Services.Interface
{
    public interface IKoiFishService
    {
        Task<PagedList<KoiFish>> GetKoiFishes(KoiParams koiParams);

        Task<KoiFishResponseDTO> GetKoiFishById(int id);

        Task<KoiFishResponseDTO> CreateKoiFish(KoiFishCreateDTO koiFishModel);

        Task<KoiFishResponseDTO> UpdateKoiFish(int id, KoiFishUpadteDTO koiFishModel);

        Task<KoiFishResponseDTO> DeleteKoiFish(int id);
    }
}