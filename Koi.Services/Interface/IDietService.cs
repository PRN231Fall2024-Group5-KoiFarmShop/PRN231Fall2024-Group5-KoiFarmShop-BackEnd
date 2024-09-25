using Koi.DTOs.DietDTOs;

namespace Koi.Services.Interface
{
    public interface IDietService
    {
        Task<List<DietCreateDTO>> GetDiets(string? searchTerm);

        Task<DietCreateDTO> GetDietById(int id);

        Task<DietCreateDTO> CreateDiet(DietCreateDTO dietModel);

        Task<DietCreateDTO> UpdateDiet(int id, DietCreateDTO dietModel);

        Task<bool> DeleteDiet(int id);
    }
}
