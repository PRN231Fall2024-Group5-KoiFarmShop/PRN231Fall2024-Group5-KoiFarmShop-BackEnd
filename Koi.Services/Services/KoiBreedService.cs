using AutoMapper;
using Koi.DTOs.KoiBreedDTOs;
using Koi.Repositories.Entities;
using Koi.Repositories.Helper;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;

namespace Koi.Services.Services
{
    public class KoiBreedService : IKoiBreedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //private readonly INotificationService _notificationService;
        private readonly IClaimsService _claimsService;

        //private readonly IRedisService _redisService;

        public KoiBreedService(
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

        public async Task<List<KoiBreedResponseDTO>> GetKoiBreeds(KoiBreedParams koiBreedParams)
        {
            List<KoiBreedResponseDTO> result;

            //// Bước 1: Kiểm tra cache
            //var cachedCategories = await _redisService.GetStringAsync(CacheKeys.EventCategories);
            //if (!string.IsNullOrEmpty(cachedCategories))
            //{
            //    // Nếu cache tồn tại, giải mã và sử dụng dữ liệu từ cache
            //    result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EventCategoryResponseDTO>>(cachedCategories);
            //}
            //else
            //{
            // Nếu cache không tồn tại, truy vấn từ cơ sở dữ liệu
            var eventCategories = await _unitOfWork.KoiBreedRepository.GetAllAsync();

            result = _mapper.Map<List<KoiBreedResponseDTO>>(eventCategories);

            //    // Lưu kết quả vào cache
            //    var serializedResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            //    await _redisService.SetStringAsync(CacheKeys.EventCategories, serializedResult, TimeSpan.FromMinutes(30)); // Cache for 30 minutes
            //}

            // Bước 2: Áp dụng tìm kiếm và sắp xếp trên kết quả
            if (!string.IsNullOrEmpty(koiBreedParams.SearchTerm))
            {
                result = result
                    .Where(x => x.Name.ToLower().Contains(koiBreedParams.SearchTerm.ToLower()))
                    .ToList();
            }

            result = result.ToList();

            return result;
        }

        public async Task<KoiBreedResponseDTO> GetKoiBreedById(int id)
        {
            //// Try to get from cache
            //var cachedCategory = await _redisService.GetStringAsync(CacheKeys.EventCategory(id));
            //if (!string.IsNullOrEmpty(cachedCategory))
            //{
            //    return Newtonsoft.Json.JsonConvert.DeserializeObject<EventCategoryResponseDTO>(cachedCategory);
            //}

            // If not in cache, query the database
            var koiBreed = await _unitOfWork.KoiBreedRepository.GetByIdAsync(id);

            if (koiBreed == null)
            {
                throw new Exception("404 - Breed not found!");
            }

            var result = _mapper.Map<KoiBreedResponseDTO>(koiBreed);

            //// Cache the result
            //var serializedResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            //await _redisService.SetStringAsync(CacheKeys.EventCategory(id), serializedResult, TimeSpan.FromMinutes(30));
            //// Cache for 30 minutes

            return result;
        }

        public async Task<KoiBreedResponseDTO> CreateKoiBreed(CreateKoiBreedDTO koiBreedModel)
        {
            // check if event category already exists
            var existingKoiBreed = await _unitOfWork.KoiBreedRepository
                .GetAllAsync();

            var isExist = existingKoiBreed.FirstOrDefault(x => x.Name.ToLower() == koiBreedModel.Name.ToLower());

            if (isExist != null)
            {
                throw new Exception("400 - Create failed. Breed has already existed!");
            }

            // create new event category
            var koiBreed = new KoiBreed
            {
                Name = koiBreedModel.Name,
                Content = koiBreedModel.Content
            };
            var newCategory = await _unitOfWork.KoiBreedRepository.AddAsync(koiBreed);

            //// Clear cache as new category is added
            //await _redisService.DeleteKeyAsync(CacheKeys.EventCategories);

            // mapper
            var result = _mapper.Map<KoiBreedResponseDTO>(newCategory);
            await _unitOfWork.SaveChangeAsync();
            return result;
        }

        public async Task<KoiBreedResponseDTO> UpdateKoiBreed(int id, CreateKoiBreedDTO koiBreedModel)
        {
            var koiBreed = _mapper.Map<KoiBreed>(await GetKoiBreedById(id));

            if (koiBreed == null)
            {
                throw new Exception("404 - Update failed. Breed not found!");
            }

            koiBreed.Name = koiBreedModel.Name;
            koiBreed.Content = koiBreedModel?.Content ?? "None";

            var isUpdated = await _unitOfWork.KoiBreedRepository.Update(koiBreed);

            if (isUpdated == false)
            {
                throw new Exception("400 - Update failed");
            }

            //// Clear specific cache key
            //await _redisService.DeleteKeyAsync(CacheKeys.EventCategory(id));
            //// Clear general list cache
            //await _redisService.DeleteKeyAsync(CacheKeys.EventCategories);

            var result = _mapper.Map<KoiBreedResponseDTO>(koiBreed);
            await _unitOfWork.SaveChangeAsync();
            return result;
        }

        public async Task<bool> DeleteKoiBreed(int id)
        {
            var koiBreed = await _unitOfWork.KoiBreedRepository.GetByIdAsync(id);

            if (koiBreed == null)
            {
                throw new Exception("404 - Delete failed. Breed not found!");
            }

            var isDeleted = await _unitOfWork.KoiBreedRepository.SoftRemove(koiBreed);
            await _unitOfWork.SaveChangeAsync();

            if (isDeleted)
            {
                //// Clear specific cache key
                //await _redisService.DeleteKeyAsync(CacheKeys.EventCategory(id));
                //// Clear general list cache
                //await _redisService.DeleteKeyAsync(CacheKeys.EventCategories);
            }

            return isDeleted;
        }
    }
}