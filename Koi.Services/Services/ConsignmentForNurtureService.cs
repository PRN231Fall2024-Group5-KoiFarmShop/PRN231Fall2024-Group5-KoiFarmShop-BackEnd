using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.ConsignmentDTOs;
using Koi.DTOs.Enums;
using Koi.Repositories.Interfaces;
using Koi.Repositories.Utils;
using Koi.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Koi.Services.Services
{
    public class ConsignmentForNurtureService : IConsignmentForNurtureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;

        public ConsignmentForNurtureService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentTime = currentTime;
        }

        public async Task<ConsignmentForNurtureDTO> CreateConsignmentAsync(ConsignmentRequestDTO consignmentRequestDTO)
        {
            try
            {
                // Get current user
                var user = await _unitOfWork.UserRepository.GetCurrentUserAsync();
                if (user == null)
                {
                    throw new Exception("401 - User is not signed in");
                }

                var koiFish = await _unitOfWork.KoiFishRepository.GetByIdAsync(consignmentRequestDTO.KoiFishId);
                if (koiFish == null)
                {
                    throw new Exception("400 - Invalid KoiFish is not existed");
                }

                var diet = await _unitOfWork.DietRepository.GetByIdAsync(consignmentRequestDTO.DietId);
                if (diet == null)
                {
                    throw new Exception($"400 - Invalid Diet Id: {consignmentRequestDTO.DietId}");
                }

                var totalDays = ResourceHelper.DateTimeValidate(consignmentRequestDTO.StartDate, consignmentRequestDTO.EndDate);
                var projectedCost = totalDays * diet.DietCost;
                var consignment = new ConsignmentForNurture
                {
                    CustomerId = user.Id,
                    KoiFishId = consignmentRequestDTO.KoiFishId,
                    DietId = consignmentRequestDTO.DietId,
                    StartDate = consignmentRequestDTO.StartDate,
                    EndDate = consignmentRequestDTO.EndDate,
                    TotalDays = totalDays,
                    DietCost = diet.DietCost,
                    ProjectedCost = projectedCost,
                    ConsignmentDate = _currentTime.GetCurrentTime(),
                    ConsignmentStatus = ConsignmentStatusEnums.PENDING.ToString(),
                    Note = consignmentRequestDTO.Note
                };

                var addedConsignment = await _unitOfWork.ConsignmentForNurtureRepository.AddNurtureConsignmentAsync(consignment);

                if (await _unitOfWork.SaveChangeAsync() <= 0)
                {
                    throw new Exception("400 - Adding consginment proccess has been failed");
                }

                return _mapper.Map<ConsignmentForNurtureDTO>(addedConsignment);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ConsignmentForNurtureDTO> GetConsignmentByIdAsync(int consignmentId)
        {
            var consignment = await _unitOfWork.ConsignmentForNurtureRepository.GetByIdAsync(consignmentId, x => x.KoiFish);
            if (consignment == null)
            {
                throw new Exception($"404 - Consignment with id {consignmentId} not found");
            }

            return _mapper.Map<ConsignmentForNurtureDTO>(consignment);
        }

        public async Task<List<ConsignmentForNurtureDTO>> GetAllConsignmentsAsync()
        {
            var consignments = await _unitOfWork.ConsignmentForNurtureRepository.GetAllAsync();
            return _mapper.Map<List<ConsignmentForNurtureDTO>>(consignments);
        }

        public async Task UpdateConsignmentStatusAsync(int consignmentId, string newStatus)
        {
            try
            {
                var consignment = await _unitOfWork.ConsignmentForNurtureRepository.GetByIdAsync(consignmentId);
                if (consignment == null)
                {
                    throw new Exception($"404 - Consignment with id {consignmentId} not found");
                }

                consignment.ConsignmentStatus = newStatus;
                consignment.ModifiedAt = _currentTime.GetCurrentTime();

                await _unitOfWork.ConsignmentForNurtureRepository.Update(consignment);
                if (await _unitOfWork.SaveChangeAsync() <= 0)
                {
                    throw new Exception("400 - saving proccess has been failed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}