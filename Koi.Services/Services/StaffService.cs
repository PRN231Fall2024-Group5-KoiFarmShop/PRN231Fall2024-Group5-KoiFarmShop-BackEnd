using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.ConsignmentDTOs;
using Koi.DTOs.Enums;
using Koi.DTOs.PaymentDTOs;
using Koi.Repositories.Commons;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Services.Services
{
    public class StaffService : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailServices _orderDetailServices;
        private readonly ICurrentTime _currentTime;

        public StaffService(IUnitOfWork unitOfWork, IMapper mapper, IOrderService orderService, IOrderDetailServices orderDetailServices, ICurrentTime currentTime)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderService = orderService;
            _orderDetailServices = orderDetailServices;
            _currentTime = currentTime;
        }

        public async Task<List<ConsignmentForNurtureDetailDTO>> GetAssignedConsigntment(int staffId)
        {
            var result = await _unitOfWork.ConsignmentForNurtureRepository.GetAssignedConsignments(staffId);
            return _mapper.Map<List<ConsignmentForNurtureDetailDTO>>(result);
        }

        public async Task<List<OrderDetailDTO>> OrderDetailDTO(int staffId)
        {
            var result = await _unitOfWork.OrderDetailRepository.GetAssignedOrderDetails(staffId);
            return _mapper.Map<List<OrderDetailDTO>>(result);
        }

        public async Task<ApiResult<OrderDetailDTO>> AssignStaffOrderDetail(int ordetailId, int staffId)
        {
            try
            {
                var detail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(ordetailId, x => x.ConsignmentForNurture, x => x.KoiFish);
                if (detail == null) throw new Exception("404 - Not Found Order Detail!");
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(detail.OrderId);
                if (order == null) throw new Exception("404 - Not Found Order");
                var staff = await _unitOfWork.UserRepository.GetAccountDetailsAsync(staffId);
                if (staff == null) throw new Exception("404 - Not found Staff");
                else
                {
                    if (staff.RoleName != "STAFF")
                    {
                        throw new Exception("400 - You can only assign staff user except other roles");
                    }
                }

                if (order.OrderStatus != OrderStatusEnums.COMPLETED.ToString() && detail.Status != OrderDetailStatusEnum.COMPLETED.ToString())
                {
                    string message = "Staff assigned successfully.";

                    if (detail.StaffId != null)
                    {
                        message = "Staff reassigned for existing order detail.";
                    }
                    detail.StaffId = staffId;
                    detail.Status = OrderDetailStatusEnum.ISSHIPPING.ToString();
                    var existingConsignment = detail.ConsignmentForNurture;
                    if (existingConsignment != null)
                    {
                        if (existingConsignment.StaffId != null)
                        {
                            message += "Staff reassigned for existing consignment.";
                        }
                        existingConsignment.StaffId = staffId;
                       // existingConsignment.StartDate = _currentTime.GetCurrentTime();
                       // existingConsignment.EndDate = _currentTime.GetCurrentTime().AddDays(existingConsignment.TotalDays.Value);
                        existingConsignment.InspectionRequired = true;
                        existingConsignment.InspectionDate = _currentTime.GetCurrentTime();
                        detail.Status = OrderDetailStatusEnum.ISNUTURING.ToString();
                        existingConsignment.ConsignmentStatus = ConsignmentStatusEnums.NURTURING.ToString();

                        await _unitOfWork.ConsignmentForNurtureRepository.Update(existingConsignment);
                    }
                    else
                    {
                        if (order.OrderStatus != OrderStatusEnums.PROCESSING.ToString())
                            order.OrderStatus = OrderStatusEnums.PROCESSING.ToString();
                    }

                    await _unitOfWork.OrderDetailRepository.Update(detail);
                    if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving");

                    return ApiResult<OrderDetailDTO>.Succeed(_mapper.Map<OrderDetailDTO>(detail), message);
                }
                else
                {

                    return ApiResult<OrderDetailDTO>.Error(null, "400 - Order status:" + order.OrderStatus + "or detail status:" + detail.Status + "is invalid");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //order details

        public async Task<ConsignmentForNurtureDTO> UpdateConsignmentStatusOnlyAsync(int consignmentId, ConsignmentStatusEnums newStatus)
        {
            try
            {
                // Lấy consignment từ cơ sở dữ liệu
                var consignment = await _unitOfWork.ConsignmentForNurtureRepository.GetByIdAsync(consignmentId);
                if (consignment == null)
                {
                    throw new Exception($"404 - Consignment with id {consignmentId} not found");
                }

                // Cập nhật trạng thái và thời gian chỉnh sửa
                consignment.ConsignmentStatus = newStatus.ToString();
                consignment.ModifiedAt = _currentTime.GetCurrentTime();

                // Lưu thay đổi
                await _unitOfWork.ConsignmentForNurtureRepository.Update(consignment);
                if (await _unitOfWork.SaveChangeAsync() <= 0)
                {
                    throw new Exception("400 - Failed to update consignment status");
                }

                return _mapper.Map<ConsignmentForNurtureDTO>(consignment);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //change order detail to complete
        public async Task<OrderDetailDTO> ChangeToCompleted(int id)
        {
            var detail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id, x => x.KoiFish);
            if (detail == null) throw new Exception("404 - Not Found Order Detail!");
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(detail.OrderId, x => x.User);
            if (order == null) throw new Exception("404 - Not Found Order");
            if (order.OrderStatus == OrderStatusEnums.PROCESSING.ToString()
                && (detail.Status == OrderDetailStatusEnum.ISSHIPPING.ToString()
                || detail.Status == OrderDetailStatusEnum.ISNUTURING.ToString()))
            {
                detail.Status = OrderDetailStatusEnum.COMPLETED.ToString();

                if (detail.KoiFish == null)
                {
                    throw new Exception($"This fish id: {detail.KoiFishId} does not exist in data");
                }

                detail.KoiFish.OwnerId = order.UserId;
                //if(detail.ConsignmentForNurture != null)
                //{
                //    if(detail.ConsignmentForNurture.ConsignmentStatus != ConsignmentStatusEnums.DONE.ToString())
                //    {
                //        throw new Exception("400 - This consignment of order detail is not finished");
                //    }
                //}
                if (order.OrderDetails.All(x => x.Status == OrderDetailStatusEnum.COMPLETED.ToString()))
                    order.OrderStatus = OrderDetailStatusEnum.COMPLETED.ToString();

                if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving");
                return _mapper.Map<OrderDetailDTO>(detail);
            }
            else
            {
                throw new Exception("400 - Order status:" + order.OrderStatus + "or detail status:" + detail.Status + "is invalid");
            }
        }

        //order details

        public async Task<OrderDetailDTO> ChangeToConsigned(int id)
        {
            var detail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id, x => x.ConsignmentForNurture);
            if (detail == null) throw new Exception("404 - Not Found Order Detail!");
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(detail.OrderId);
            if (order == null) throw new Exception("404 - Not Found Order");

            if (order.OrderStatus != OrderStatusEnums.COMPLETED.ToString()
                && detail.Status != OrderDetailStatusEnum.COMPLETED.ToString()
                )
            {
                detail.Status = OrderDetailStatusEnum.ISNUTURING.ToString();
                if (detail.ConsignmentForNurture != null)
                {
                    if (detail.ConsignmentForNurture.ConsignmentStatus != ConsignmentStatusEnums.DONE.ToString())
                    {
                        throw new Exception("400 - this consignment of order detail is not finished");
                    }
                }

                if (order.OrderStatus == OrderStatusEnums.PENDING.ToString())
                    order.OrderStatus = OrderStatusEnums.PROCESSING.ToString();
                if (order.OrderDetails.All(x => x.Status == OrderDetailStatusEnum.COMPLETED.ToString() ))
                    order.OrderStatus = OrderDetailStatusEnum.COMPLETED.ToString();
                if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving");
                return _mapper.Map<OrderDetailDTO>(detail);
            }
            else
            {
                throw new Exception("400 - Order status:" + order.OrderStatus + "or detail status:" + detail.Status + "is invalid");
            }
        }

        //order details
        public async Task<OrderDetailDTO> ChangeToShipping(int id)
        {
            var detail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
            if (detail == null) throw new Exception("404 - Not Found Order Detail!");
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(detail.OrderId);
            if (order == null) throw new Exception("404 - Not Found Order");

            if (order.OrderStatus != OrderStatusEnums.COMPLETED.ToString()
                          && detail.Status != OrderDetailStatusEnum.COMPLETED.ToString()
                )
            {
                detail.Status = OrderDetailStatusEnum.ISSHIPPING.ToString();
                if (order.OrderStatus != OrderStatusEnums.PROCESSING.ToString())
                    order.OrderStatus = OrderStatusEnums.PROCESSING.ToString();
                if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving");
                return _mapper.Map<OrderDetailDTO>(detail);
            }
            else
            {
                throw new Exception("400 - Order status:"+order.OrderStatus+"or detail status:"+detail.Status+ "is invalid"  );
            }
        }

        public async Task<OrderDetailDTO> ChangeToGettingFish(int id)
        {
            var detail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
            if (detail == null) throw new Exception("404 - Not Found Order Detail!");
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(detail.OrderId);
            if (order == null) throw new Exception("404 - Not Found Order");

            if ((order.OrderStatus != OrderStatusEnums.COMPLETED.ToString()
                          && detail.Status != OrderDetailStatusEnum.COMPLETED.ToString()) && detail.Status == OrderDetailStatusEnum.PENDING.ToString()
                )
            {
                detail.Status = OrderDetailStatusEnum.GETTINGFISH.ToString();
                if (order.OrderStatus != OrderStatusEnums.PROCESSING.ToString())
                    order.OrderStatus = OrderStatusEnums.PROCESSING.ToString();
                if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving");
                return _mapper.Map<OrderDetailDTO>(detail);
            }
            else
            {
                throw new Exception("400 - Order status or detail status is invalid");
            }
        }
    }
}