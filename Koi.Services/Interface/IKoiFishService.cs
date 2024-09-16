using Koi.BusinessObjects;
using Koi.DTOs.KoiFishDTOs;
using Koi.Repositories.Helper;

namespace Koi.Services.Interface
{
    public interface IKoiFishService
    {
        Task<PagedList<KoiFish>> GetKoiFishes(KoiParams koiParams);

        Task<KoiFishResponseDTO> GetKoiFishById(int id);

        Task<KoiFishResponseDTO> CreateKoiFish(CreateKoiFishDTO koiFishModel);

        Task<KoiFishResponseDTO> UpdateKoiFish(int id, UpdateKoiFishDTO koiFishModel);

        Task<KoiFishResponseDTO> DeleteKoiFish(int id);
    }
}