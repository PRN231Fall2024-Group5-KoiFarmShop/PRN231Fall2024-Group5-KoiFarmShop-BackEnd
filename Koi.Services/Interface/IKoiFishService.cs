﻿using Koi.BusinessObjects;
using Koi.DTOs.KoiFishDTOs;
using Koi.Repositories.Helper;
using Koi.Services.Interface;

namespace Koi.Services.Interface
{
    public interface IKoiFishService
    {
        IQueryable<KoiFish> GetKoiFishes();
        IQueryable<KoiFish> GetMyKoiFishes();
        Task<PagedList<KoiFish>> GetKoiFishes(KoiParams koiParams);
        Task<KoiFishResponseDTO> GetKoiFishById(int id);

        Task<KoiFishResponseDTO> CreateKoiFish(KoiFishCreateDTO koiFishModel);

        Task<KoiFishResponseDTO> UpdateKoiFish(int id, KoiFishUpdateDTO koiFishModel);

        Task<KoiFishResponseDTO> DeleteKoiFish(int id);
        Task<bool> UpdateConsign(int id, int consignedBy);
        Task<bool> EndConsigned(int id);
    }
}