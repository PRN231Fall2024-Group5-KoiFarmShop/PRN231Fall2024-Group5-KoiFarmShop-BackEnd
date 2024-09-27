using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.DietDTOs;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;

namespace Koi.Services.Services
{
    public class DietService : IDietService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //private readonly INotificationService _notificationService;
        private readonly IClaimsService _claimsService;

        //private readonly IRedisService _redisService;

        public DietService(
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
        public async Task<DietCreateDTO> CreateDiet(DietCreateDTO dietModel)
        {
            var result = await _unitOfWork.DietRepository.AddAsync(_mapper.Map<Diet>(dietModel));
            if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail Saing!");
            return _mapper.Map<DietCreateDTO>(result);
        }

        public async Task<bool> DeleteDiet(int id)
        {
            var tar = await _unitOfWork.DietRepository.GetByIdAsync(id);
            if (tar == null) throw new Exception("404 - Diet not Found!");
            tar.IsDeleted = true;
            if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail Saving!");
            return true;
        }

        public async Task<DietCreateDTO> GetDietById(int id)
        {
            var tar = await _unitOfWork.DietRepository.GetByIdAsync(id);
            if (tar == null) throw new Exception("404 - Diet not Found!");
            return _mapper.Map<DietCreateDTO>(tar);
        }

        public async Task<List<DietCreateDTO>> GetDiets(string? searchTerm)
        {
            var list = await _unitOfWork.DietRepository.GetAllAsync(x => x.IsDeleted == false && (x.Name.ToLower().Contains(searchTerm.ToLower()) || x.Description.ToLower().Contains(searchTerm.ToLower())));
            if (list == null) return new();
            return _mapper.Map<List<DietCreateDTO>>(list);
        }

        public async Task<DietCreateDTO> UpdateDiet(int id, DietCreateDTO dietModel)
        {
            var tar = await _unitOfWork.DietRepository.GetByIdAsync(id);
            if (tar == null) throw new Exception("404 - Diet not Found!");
            tar.Name = dietModel.Name;
            tar.Description = dietModel.Description;
            tar.DietCost = dietModel.DietCost;
            if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail Saving!");
            return _mapper.Map<DietCreateDTO>(tar);
        }
    }
}
