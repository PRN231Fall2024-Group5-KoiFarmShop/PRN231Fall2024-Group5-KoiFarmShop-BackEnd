using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.ConsignmentDTOs;
using Koi.DTOs.Enums;
using Koi.DTOs.PaymentDTOs;
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
                    ActualCost = projectedCost,
                    ConsignmentDate = _currentTime.GetCurrentTime(),
                    ConsignmentStatus = ConsignmentStatusEnums.PENDING.ToString(),
                    Note = consignmentRequestDTO.Note
                };

                var addedConsignment = await _unitOfWork.ConsignmentForNurtureRepository.AddAsync(consignment);

                //if (await _unitOfWork.SaveChangeAsync() <= 0)
                //{
                //    throw new Exception("400 - Adding consginment proccess has been failed");
                //}

                //Create order for this consignment

                var newOrder = new Order
                {
                    UserId = user.Id,
                    TotalAmount = projectedCost,
                    OrderStatus = OrderStatusEnums.PENDING.ToString(),
                    ShippingAddress = "",
                    PaymentMethod = "Order create for consignment",
                    IsConsignmentIncluded = true,
                };
                newOrder = await _unitOfWork.OrderRepository.AddAsync(newOrder);

                var personalWallet = await _unitOfWork.WalletRepository.GetWalletByUserId(user.Id);
                var wallet = user.Wallet;
                if (personalWallet == null)
                {
                    throw new Exception("Personal Wallet not found+");
                }
                // Check balance

                if (personalWallet.Balance < newOrder.TotalAmount)
                {
                    newOrder.OrderStatus = OrderStatusEnums.CANCELLED.ToString();
                    newOrder.Note = "Not enough wallet to pay";

                    consignment.ConsignmentStatus = OrderStatusEnums.CANCELLED.ToString();
                    consignment.Note = "Not enough wallet to pay";

                    if (await _unitOfWork.SaveChangeAsync() <= 0)
                    {
                        throw new Exception("400 - Adding consginment and create order proccess has been failed");
                    }
                    throw new Exception("You dont have enough money to purchase this order");
                }
                if (await _unitOfWork.SaveChangeAsync() <= 0)
                {
                    throw new Exception("400 - Adding consginment and create order proccess has been failed");
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

        public async Task<ConsignmentForNurtureDTO> UpdateConsignmentAsync(int consignmentId, ConsignmentUpdateDTO consignmentUpdateDTO)
        {
            try
            {
                // Retrieve the existing consignment
                var existingConsignment = await _unitOfWork.ConsignmentForNurtureRepository.GetByIdAsync(consignmentId);
                if (existingConsignment == null)
                {
                    throw new Exception($"404 - Consignment with id {consignmentId} not found");
                }

                // Update fields if they are provided
                if (consignmentUpdateDTO.DietId.HasValue)
                {
                    var diet = await _unitOfWork.DietRepository.GetByIdAsync(consignmentUpdateDTO.DietId.Value);
                    if (diet == null)
                    {
                        throw new Exception($"400 - Invalid Diet Id: {consignmentUpdateDTO.DietId.Value}");
                    }
                    existingConsignment.DietId = consignmentUpdateDTO.DietId;
                    existingConsignment.DietCost = diet.DietCost; // Update diet cost based on the new diet
                }

                if (consignmentUpdateDTO.StaffId.HasValue)
                {
                    existingConsignment.StaffId = consignmentUpdateDTO.StaffId;
                }

                existingConsignment.StartDate = consignmentUpdateDTO.StartDate != DateTime.MinValue
                    ? consignmentUpdateDTO.StartDate
                    : existingConsignment.StartDate;

                existingConsignment.EndDate = consignmentUpdateDTO.EndDate != DateTime.MinValue
                    ? consignmentUpdateDTO.EndDate
                    : existingConsignment.EndDate;

                // Recalculate projected cost if dates are changed
                if (consignmentUpdateDTO.StartDate != DateTime.MinValue || consignmentUpdateDTO.EndDate != DateTime.MinValue)
                {
                    var totalDays = ResourceHelper.DateTimeValidate(existingConsignment.StartDate.Value, existingConsignment.EndDate.Value);
                    existingConsignment.TotalDays = totalDays;
                    existingConsignment.ProjectedCost = totalDays * existingConsignment.DietCost;
                }

                existingConsignment.LaborCost = consignmentUpdateDTO.LaborCost ?? existingConsignment.LaborCost;
                existingConsignment.ActualCost = consignmentUpdateDTO.ActualCost ?? existingConsignment.ActualCost;
                existingConsignment.ConsignmentStatus = consignmentUpdateDTO.ConsignmentStatus.ToString();
                existingConsignment.Note = consignmentUpdateDTO.Note ?? existingConsignment.Note;
                existingConsignment.InspectionRequired = consignmentUpdateDTO.InspectionRequired ?? existingConsignment.InspectionRequired;
                existingConsignment.InspectionDate = consignmentUpdateDTO.InspectionDate ?? existingConsignment.InspectionDate;
                existingConsignment.ModifiedAt = _currentTime.GetCurrentTime();

                // Update the consignment
                await _unitOfWork.ConsignmentForNurtureRepository.Update(existingConsignment);
                if (await _unitOfWork.SaveChangeAsync() <= 0)
                {
                    throw new Exception("400 - Saving process failed");
                }

                // Return the updated consignment as DTO
                return _mapper.Map<ConsignmentForNurtureDTO>(existingConsignment);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}