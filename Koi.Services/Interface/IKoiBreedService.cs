using Koi.BusinessObjects.DTO.KoiBreedDTOs;
using Koi.Repositories.Helper;

namespace Koi.Services.Interface
{
    public interface IKoiBreedService
    {
        Task<List<KoiBreedResponseDTO>> GetKoiBreeds(KoiBreedParams koiBreedParams);

        Task<KoiBreedResponseDTO> GetKoiBreedById(int id);

        Task<KoiBreedResponseDTO> CreateKoiBreed(CreateKoiBreedDTO koiBreedModel);

        Task<KoiBreedResponseDTO> UpdateKoiBreed(int id, CreateKoiBreedDTO koiBreedModel);

        Task<bool> DeleteKoiBreed(int id);
    }
}