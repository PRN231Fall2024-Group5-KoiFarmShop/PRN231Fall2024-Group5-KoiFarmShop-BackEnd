﻿using Koi.DTOs.KoiFishDTOs;

namespace Koi.Services.Interface
{
    public interface IKoiDiaryService
    {
        Task<KoiFishDiaryCreateDTO> CreateDiary(KoiFishDiaryCreateDTO koiDiary);

        Task<KoiFishDiaryCreateDTO> UpdateDiary(int id, KoiFishDiaryUpdateDTO koiDiary);
    }
}
