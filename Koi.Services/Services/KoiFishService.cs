using AutoMapper;
using Koi.DTOs;
using Koi.DTOs.KoiFishDTOs;
using Koi.Repositories.Entities;
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
                x => x.KoiFishKoiBreeds,
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
                throw new Exception("404 Fish not found!");
            }

            var result = _mapper.Map<KoiFishResponseDTO>(fish);

            //// Cache the result
            //var serializedResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            //await _redisService.SetStringAsync(CacheKeys.Event(id), serializedResult, TimeSpan.FromMinutes(30));
            // Cache for 30 minutes

            return result;
        }

        public async Task<KoiFishResponseDTO> CreateKoiFish(CreateKoiFishDTO fishModel)
        {
            throw new NotImplementedException("501 - Not Implemented");
            //var fishEntity = _mapper.Map<KoiFishResponseDTO>(fishModel);
            //////check user
            ////Guid userId = _claimsService.GetCurrentUserId;
            ////var isExistUser = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            ////if (isExistUser == null)
            ////{
            ////    throw new Exception("User does not exist!");
            ////}
            ////eventEntity.UserId = isExistUser.Id;
            ////eventEntity.User = isExistUser;
            ////check koiBreed
            //var eventCategory = await _unitOfWork.KoiBreedRepository.GetByIdAsync(fishModel.);
            //if (eventCategory == null)
            //{
            //    throw new Exception("Event category not null when create event");
            //}
            //eventEntity.EventCategory = eventCategory;

            ////eventEntity.Name = eventModel.Name;
            ////eventEntity.Description = eventModel.Description;
            ////eventEntity.ThumbnailUrl = eventModel.ThumbnailUrl;
            ////eventEntity.EventStartDate = eventModel.EventStartDate;
            ////eventEntity.EventEndDate = eventModel.EventEndDate;

            //eventEntity.Status = EventStatusEnums.DRAFT.ToString();

            //var newEvent = await _unitOfWork.EventRepository.AddAsync(eventEntity);

            //var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            //var result = _mapper.Map<EventResponseDTO>(newEvent);

            //if (isSuccess)
            //{
            //    //    await _notificationService.PushNotificationToManager(new Notification
            //    //    {
            //    //        Title = "User " + isExistUser.Email + " Has Created Event",
            //    //        Body = $"Event Name: " + eventModel.Name,
            //    //        Url = "/dashboard/feedback/event/" + newEvent.Id,
            //    //    });

            //    // Clear cache as new category is added
            //    await _redisService.DeleteKeyAsync(CacheKeys.Events);
            //    return result;
            //}
            //else
            //{
            //    throw new Exception("Failed to create event");
            //}
        }

        public async Task<KoiFishResponseDTO> UpdateKoiFish(int id, CreateKoiFishDTO eventModel)
        {
            throw new NotImplementedException("501 - Not Implemented");
            //var existingEvent = await _unitOfWork.EventRepository.GetByIdAsync(id);

            //if (existingEvent == null)
            //{
            //    throw new Exception("Event not found");
            //}
            ////check user
            ////var user = await _unitOfWork.UserRepository.GetAllUsersAsync();
            ////var isExistUser = user.FirstOrDefault(x => x.Id == eventModel.UserId);
            ////if (isExistUser == null)
            ////{
            ////    throw new Exception("User does not exist!");
            ////}
            ////existingEvent.User = isExistUser;
            ////check eventCategory
            //var eventCategory = await _unitOfWork.EventCategoryRepository.GetByIdAsync(eventModel.EventCategoryId);
            //if (eventCategory == null)
            //{
            //    throw new Exception("Event category does not exist!");
            //}
            //existingEvent.EventCategory = eventCategory;

            //existingEvent.Name = eventModel.Name ?? existingEvent.Name;
            //existingEvent.Description = eventModel.Description ?? existingEvent.Description;
            //existingEvent.ThumbnailUrl = eventModel.ThumbnailUrl ?? existingEvent.ThumbnailUrl;
            //existingEvent.EventStartDate = eventModel.EventStartDate ?? existingEvent.EventStartDate;
            //existingEvent.EventEndDate = eventModel.EventEndDate ?? existingEvent.EventEndDate;
            //existingEvent.Status = eventModel.Status.ToString() ?? existingEvent.Status;

            //var isUpdated = await _unitOfWork.EventRepository.Update(existingEvent);

            //if (isUpdated == false)
            //{
            //    throw new Exception("Failed to update event");
            //}

            //await _unitOfWork.SaveChangeAsync();
            //// Clear specific cache key
            //await _redisService.DeleteKeyAsync(CacheKeys.Event(id));
            //// Clear general list cache
            //await _redisService.DeleteKeyAsync(CacheKeys.Events);
            //var result = _mapper.Map<EventResponseDTO>(existingEvent);
            //return result;
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