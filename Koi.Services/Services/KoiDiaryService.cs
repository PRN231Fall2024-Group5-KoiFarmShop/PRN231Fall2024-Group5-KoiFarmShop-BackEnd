using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.KoiDiaryDTOs;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;

namespace Koi.Services.Services
{
    public class KoiDiaryService : IKoiDiaryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //private readonly INotificationService _notificationService;
        private readonly IClaimsService _claimsService;

        //private readonly IRedisService _redisService;

        public KoiDiaryService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            //INotificationService notificationService,
            IClaimsService claimsService
        //IRedisService redisService
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_notificationService = notificationService;
            _claimsService = claimsService;
            //_redisService = redisService;
        }

        public async Task<KoiFishDiaryCreateDTO> CreateDiary(KoiFishDiaryCreateDTO koiDiary)
        {
            try
            {
                var fish = await _unitOfWork.KoiFishRepository.GetByIdAsync(koiDiary.KoiFishId);
                if (fish == null)
                    throw new Exception("400 - Fish not Found!");
                var list = await _unitOfWork.KoiDiaryRepository.GetAllAsync();
                var item = list.FirstOrDefault(x => x.KoiFishId == koiDiary.KoiFishId && x.Date.Date == koiDiary.Date.Date);
                if (item != null) throw new Exception("400 - Fish has already have diary on this date");
                var tmp = _unitOfWork.KoiDiaryRepository.AddAsync(_mapper.Map<KoiDiary>(koiDiary));
                if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving changes");
                return koiDiary;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<KoiDiary> GetDiaryList() => _unitOfWork.KoiDiaryRepository.GetQueryable();

        public async Task<KoiFishDiaryCreateDTO> UpdateDiary(int id, KoiFishDiaryUpdateDTO koiDiary)
        {
            try
            {
                var item = await _unitOfWork.KoiDiaryRepository.GetByIdAsync(id);
                if (item.Date.AddDays(3).Date < DateTime.Now.Date) throw new Exception("400 - Update time is over!");
                item.Description = koiDiary.Description;
                if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving changes");
                return _mapper.Map<KoiFishDiaryCreateDTO>(await _unitOfWork.KoiDiaryRepository.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                throw ex; ;
            }
        }
        public async Task<KoiFishDiaryCreateDTO> DeleteDiary(int id)
        {
            try
            {
                var item = await _unitOfWork.KoiDiaryRepository.GetByIdAsync(id);
                if (item.Date.AddDays(3).Date < DateTime.Now.Date) throw new Exception("400 - Update time is over!");
                var isDeleted = await _unitOfWork.KoiDiaryRepository.SoftRemove(item);
                if (!isDeleted || await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving changes");
                return _mapper.Map<KoiFishDiaryCreateDTO>(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
