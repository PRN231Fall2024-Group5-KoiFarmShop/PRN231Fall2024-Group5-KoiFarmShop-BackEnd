using Koi.BusinessObjects;
using Koi.DTOs.KoiDiaryDTOs;

namespace Koi.Services.Interface
{
    public interface IKoiDiaryService
    {
        Task<KoiFishDiaryCreateDTO> CreateDiary(KoiFishDiaryCreateDTO koiDiary);
        IQueryable<KoiDiary> GetDiaryList();
        Task<KoiFishDiaryCreateDTO> UpdateDiary(int id, KoiFishDiaryUpdateDTO koiDiary);
        Task<KoiFishDiaryCreateDTO> DeleteDiary(int id);
    }
}
