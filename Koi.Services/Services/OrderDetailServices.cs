using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.Enums;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;

namespace Koi.Services.Services
{
    public class OrderDetailServices : IOrderDetailServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderDetailServices _orderDetailServices;

        //private readonly INotificationService _notificationService;
        private readonly IClaimsService _claimsService;

        //private readonly IRedisService _redisService;

        public OrderDetailServices(
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

        //public async Task<Order> ChangeToCanceled(int id)
        //{
        //    throw new NotImplementedException("501");
        //    var detail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
        //    if (detail == null) throw new Exception("404 - Not Found Order Detail!");
        //    var order = await _unitOfWork.OrderRepository.GetByIdAsync(detail.OrderId);
        //    if (order == null) throw new Exception("404 - Not Found Order");

        //    if (order.OrderStatus == "PENDING" && detail.Status == "PENDING")
        //    {
        //        detail.Status = "CANCELED";
        //        if (order.OrderDetails.All(x => x.Status == "COMPLETED" || x.Status == "CONSIGNED"))
        //            order.OrderStatus = "COMPLETED";
        //        if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving");
        //        return order;
        //    }
        //    else
        //    {
        //        throw new Exception("400 - Order status or detail status is invalid");
        //    }
        //}

        public async Task<Order> ChangeToCompleted(int id)
        {
            var detail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
            if (detail == null) throw new Exception("404 - Not Found Order Detail!");
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(detail.OrderId, x => x.OrderDetails);
            if (order == null) throw new Exception("404 - Not Found Order");
            if (order.OrderStatus == OrderStatusEnums.PROCESSING.ToString() && detail.Status == OrderDetailStatusEnum.ISSHIPPING.ToString())
            {
                detail.Status = OrderDetailStatusEnum.COMPLETED.ToString();
                if (order.OrderDetails.All(x => x.Status == OrderDetailStatusEnum.COMPLETED.ToString()))
                    order.OrderStatus = OrderDetailStatusEnum.COMPLETED.ToString();
                if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving");
                return order;
            }
            else
            {
                throw new Exception("400 - Order status or detail status is invalid");
            }
        }

        public async Task<Order> ChangeToConsigned(int id)
        {
            var detail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
            if (detail == null) throw new Exception("404 - Not Found Order Detail!");
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(detail.OrderId, x => x.OrderDetails);
            if (order == null) throw new Exception("404 - Not Found Order");

            if ((order.OrderStatus == OrderStatusEnums.PENDING.ToString() || order.OrderStatus == OrderStatusEnums.PROCESSING.ToString()) && detail.Status == OrderDetailStatusEnum.PENDING.ToString())
            {
                detail.Status = OrderDetailStatusEnum.ISNUTURING.ToString();
                if (order.OrderStatus == OrderStatusEnums.PENDING.ToString())
                    order.OrderStatus = OrderStatusEnums.PROCESSING.ToString();
                if (order.OrderDetails.All(x => x.Status == OrderDetailStatusEnum.COMPLETED.ToString() || x.Status == OrderDetailStatusEnum.ISNUTURING.ToString()))
                    order.OrderStatus = OrderDetailStatusEnum.COMPLETED.ToString();
                if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving");
                return order;
            }
            else
            {
                throw new Exception("400 - Order status or detail status is invalid");
            }
        }

        public async Task<Order> ChangeToShipping(int id)
        {
            var detail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
            if (detail == null) throw new Exception("404 - Not Found Order Detail!");
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(detail.OrderId, x => x.OrderDetails);
            if (order == null) throw new Exception("404 - Not Found Order");

            if ((order.OrderStatus == OrderStatusEnums.PENDING.ToString() || order.OrderStatus == OrderStatusEnums.PROCESSING.ToString()) && detail.Status == OrderDetailStatusEnum.PENDING.ToString())
            {
                detail.Status = OrderDetailStatusEnum.ISSHIPPING.ToString();
                if (order.OrderStatus != OrderStatusEnums.PROCESSING.ToString())
                    order.OrderStatus = OrderStatusEnums.PROCESSING.ToString();
                if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving");
                return order;
            }
            else
            {
                throw new Exception("400 - Order status or detail status is invalid");
            }
        }

        public async Task<Order> AssignStaffOrderDetail(int id, int staffId)
        {
            try
            {
                var detail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id, x => x.ConsignmentForNurture);
                if (detail == null) throw new Exception("404 - Not Found Order Detail!");
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(detail.OrderId, x => x.OrderDetails);
                if (order == null) throw new Exception("404 - Not Found Order!");
                var staff = await _unitOfWork.UserRepository.GetAccountDetailsAsync(staffId);
                if (staff == null) throw new Exception("404 - Not Found staff!");
                if ((order.OrderStatus == OrderStatusEnums.PENDING.ToString() || order.OrderStatus == OrderStatusEnums.PROCESSING.ToString()) && detail.Status == OrderDetailStatusEnum.PENDING.ToString())
                {
                    detail.Status = OrderDetailStatusEnum.ISSHIPPING.ToString();
                    if (order.OrderStatus != OrderStatusEnums.PROCESSING.ToString())
                        order.OrderStatus = OrderStatusEnums.PROCESSING.ToString();
                    detail.StaffId = staffId;
                    await _unitOfWork.OrderDetailRepository.Update(detail);
                    if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving");
                    return _mapper.Map<Order>(order);
                }
                else
                {
                    throw new Exception("400 - Order status or detail status is invalid");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}