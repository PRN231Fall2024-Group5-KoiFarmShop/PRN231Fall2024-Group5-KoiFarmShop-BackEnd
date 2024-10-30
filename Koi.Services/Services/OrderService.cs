using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.Enums;
using Koi.DTOs.PaymentDTOs;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;
using Microsoft.Extensions.Configuration;

namespace Koi.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;

        public OrderService(IConfiguration configuration, IMapper mapper, IClaimsService claimsService, IUnitOfWork unitOfWork, ICurrentTime currentTime)
        {
            _configuration = configuration;
            _mapper = mapper;
            _claimsService = claimsService;
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
        }

        public async Task<OrderDTO> NewOrderAsync(VnpayOrderInfo orderInfo)
        {
            //var user = await _unitOfWork.UserRepository.GetCurrentUserAsync();
            //if (user == null)
            //{
            //    throw new Exception("401 - User not existing");
            //}

            var newOrder = new Order
            {
                UserId = 2,
                TotalAmount = orderInfo.Amount,
                OrderStatus = "PENDING",
                ShippingAddress = "Tron tron",
                PaymentMethod = "VNPAY",
                Note = "Test"
            };

            newOrder = await _unitOfWork.OrderRepository.AddAsync(newOrder);
            var check = await _unitOfWork.SaveChangeAsync();
            if (check > 0)
            {
                orderInfo.CommonId = newOrder.Id;
                var result = _mapper.Map<OrderDTO>(newOrder);
                return result;
            }
            else
            {
                throw new Exception("400 - Adding proccess has been failed");
            }
        }

        public async Task<List<OrderDTO>> GetOrdersAsync()
        {
            return _mapper.Map<List<OrderDTO>>(await _unitOfWork.OrderRepository.GetAllOrdersAsync());
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int orderId)
        {
            return _mapper.Map<OrderDTO>(await _unitOfWork.OrderRepository.GetOrdersById(orderId));
        }

        public async Task<List<OrderDTO>> GetOrdersByUserIdAsync(int userId)
        {
            return _mapper.Map<List<OrderDTO>>(await _unitOfWork.OrderRepository.GetOrdersByUserId(userId)); ;
        }

        public async Task<OrderDTO> CancelOrderAsync(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetOrdersById(id);
            if (order == null)
            {
                throw new Exception("404 - Order not Found");
            }
            if (order.OrderStatus == OrderStatusEnums.PENDING.ToString())
            {
                order.OrderStatus = OrderStatusEnums.REFUNDED.ToString();
                foreach (var item in order.OrderDetails)
                {
                    item.Status = OrderDetailStatusEnum.CANCELED.ToString();
                    item.KoiFish.IsAvailableForSale = true;
                }
                //Need works!! Add refund transaction

                if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("500 - Fail Saving");
            }
            else throw new Exception("400 - Invalid Order status");
            return _mapper.Map<OrderDTO>(order);
        }
    }
}