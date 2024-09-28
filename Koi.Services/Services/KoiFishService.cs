using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.KoiFishDTOs;
using Koi.Repositories.Helper;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;

namespace Koi.Services.Services
{
    public class KoiFishService : IKoiFishService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //private readonly INotificationService _notificationService;
        private readonly IClaimsService _claimsService;

        //private readonly IRedisService _redisService;

        public KoiFishService(
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

        public async Task<PagedList<KoiFish>> GetKoiFishes(KoiParams koiParams)
        {
            var query = _unitOfWork.KoiFishRepository.FilterAllField(koiParams).AsQueryable();

            var fishes = await PagedList<KoiFish>.ToPagedList(query, koiParams.PageNumber, koiParams.PageSize);

            return fishes;
        }

        public async Task<KoiFishResponseDTO> GetKoiFishByIdOld(int id)
        {
            var existingFishes = await _unitOfWork.KoiFishRepository.GetByIdAsync(id,
                //x => x.KoiFishKoiBreeds,
                x => x.Consigner
            );

            if (existingFishes == null)
            {
                throw new Exception("404 - Fish not found!");
            }

            var result = _mapper.Map<KoiFishResponseDTO>(existingFishes);
            return result;
        }

        public async Task<KoiFishResponseDTO> GetKoiFishById(int id)
        {
            //// Try to get from cache
            //var cachedOrder = await _redisService.GetStringAsync(CacheKeys.Event(id));
            //if (!string.IsNullOrEmpty(cachedOrder))
            //{
            //    return Newtonsoft.Json.JsonConvert.DeserializeObject<EventResponseDTO>(cachedOrder);
            //}

            // If not in cache, query the database
            var fish = await _unitOfWork.KoiFishRepository.GetByIdAsync(id);

            if (fish == null)
            {
                throw new Exception("404 - Fish not found!");
            }

            var result = _mapper.Map<KoiFishResponseDTO>(fish);

            //// Cache the result
            //var serializedResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            //await _redisService.SetStringAsync(CacheKeys.Event(id), serializedResult, TimeSpan.FromMinutes(30));
            // Cache for 30 minutes
            return result;
        }

        public async Task<KoiFishResponseDTO> CreateKoiFish(KoiFishCreateDTO fishModel)
        {
            ////check user
            //Guid userId = _claimsService.GetCurrentUserId;
            //var isExistUser = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            //if (isExistUser == null)
            //{
            //    throw new Exception("User does not exist!");
            //}
            //eventEntity.UserId = isExistUser.Id;
            //eventEntity.User = isExistUser;
            //check koiBreed
            KoiFish fish = _mapper.Map<KoiFish>(fishModel);
            fish.KoiBreeds = [];
            foreach (var breedId in fishModel.KoiBreedIds)
            {
                var breed = await _unitOfWork.KoiBreedRepository.GetByIdAsync(breedId);
                if (breed == null)
                {
                    throw new Exception("400 - Invalid Koi breed");
                }
                fish.KoiBreeds.Add(_mapper.Map<KoiBreed>(breed));
            }
            fish.KoiFishImages = [];
            foreach (var item in fishModel.ImageUrl)
            {
                fish.KoiFishImages.Add(new KoiFishImage
                {
                    ImageUrl = item
                });
            }
            fish.IsAvailableForSale = fishModel.IsAvailableForSale;
            fish.IsSold = fishModel.IsSold;
            fish.IsConsigned = fishModel.IsConsigned;
            fish.IsDeleted = false;

            var result = await _unitOfWork.KoiFishRepository.AddAsync(fish);

            if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving changes!");
            return _mapper.Map<KoiFishResponseDTO>(result);
        }

        public async Task<KoiFishResponseDTO> UpdateKoiFish(int id, KoiFishUpdateDTO fishModel)
        {
            ////check user
            //Guid userId = _claimsService.GetCurrentUserId;
            //var isExistUser = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            //if (isExistUser == null)
            //{
            //    throw new Exception("User does not exist!");
            //}
            //eventEntity.UserId = isExistUser.Id;
            //eventEntity.User = isExistUser;
            //check koiBreed
            KoiFish fish = await _unitOfWork.KoiFishRepository.GetByIdAsync(id);
            fish.KoiBreeds.Clear();
            foreach (var breedId in fishModel.KoiBreedIds)
            {
                var breed = await _unitOfWork.KoiBreedRepository.GetByIdAsync(breedId);
                if (breed == null)
                {
                    throw new Exception("400 - Invalid Koi breed");
                }
                fish.KoiBreeds.Add(_mapper.Map<KoiBreed>(breed));
            }
            fish.Length = fishModel.Length;
            fish.Weight = fishModel.Weight;
            //fish.KoiCertificates = fishModel.Certificate
            fish.Age = fishModel.Age;
            fish.Origin = fishModel.Origin;
            fish.DailyFeedAmount = fishModel.DailyFeedAmount;
            fish.Gender = fishModel.Gender;
            fish.LastHealthCheck = fishModel.LastHealthCheck;
            fish.PersonalityTraits = fishModel.PersonalityTraits;
            fish.Name = fishModel.Name;
            fish.IsAvailableForSale = fishModel.IsAvailableForSale;
            fish.IsSold = fishModel.IsSold;
            fish.IsConsigned = fishModel.IsConsigned;
            fish.IsDeleted = fishModel.IsDeleted;
            foreach (var item in fish.KoiFishImages)
            {
                item.IsDeleted = true;
            }
            foreach (var item in fishModel.ImageUrl)
            {
                var tmp = await _unitOfWork.KoiImageRepository.GetByUrl(item);
                if (tmp == null)
                    fish.KoiFishImages.Add(new KoiFishImage
                    {
                        ImageUrl = item
                    });
                else
                {
                    tmp.IsDeleted = false;
                }
            }
            if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving changes!");
            return _mapper.Map<KoiFishResponseDTO>(fish);
        }

        public async Task<KoiFishResponseDTO> DeleteKoiFish(int id)
        {
            var fish = await _unitOfWork.KoiFishRepository.GetByIdAsync(id);

            if (fish == null)
            {
                throw new Exception("Fish not found!");
            }

            var isDeleted = await _unitOfWork.KoiFishRepository.SoftRemove(fish);
            if (isDeleted)
            {
                var result = _mapper.Map<KoiFishResponseDTO>(fish);
                await _unitOfWork.SaveChangeAsync();
                //// Clear specific cache key
                //await _redisService.DeleteKeyAsync(CacheKeys.Event(id));
                //// Clear general list cache
                //await _redisService.DeleteKeyAsync(CacheKeys.Events);
                return result;
            }
            else
            {
                throw new Exception("Failed to delete fish");
            }
        }
    }
}