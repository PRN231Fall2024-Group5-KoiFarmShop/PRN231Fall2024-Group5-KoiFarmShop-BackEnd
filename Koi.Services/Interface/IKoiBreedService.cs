using Koi.BusinessObjects;
using Koi.DTOs.KoiBreedDTOs;
using Koi.Repositories.Helper;

namespace Koi.Services.Interface
{
    public interface IKoiBreedService
    {
        IQueryable<KoiBreed> GetKoiBreeds();
        Task<List<KoiBreedResponseDTO>> GetKoiBreeds(KoiBreedParams koiBreedParams);
        Task<KoiBreedResponseDTO> GetKoiBreedById(int id);

        Task<KoiBreedResponseDTO> CreateKoiBreed(KoiBreedCreateDTO koiBreedModel);

        Task<KoiBreedResponseDTO> UpdateKoiBreed(int id, KoiBreedCreateDTO koiBreedModel);

        Task<bool> DeleteKoiBreed(int id);
    }
}