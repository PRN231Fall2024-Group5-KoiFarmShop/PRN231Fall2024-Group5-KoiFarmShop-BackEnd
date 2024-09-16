using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.PaymentDTOs;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IVnPayService _vpnPayService;

        public PaymentService(IConfiguration configuration, IMapper mapper, IClaimsService claimsService, IUnitOfWork unitOfWork, ICurrentTime currentTime, IVnPayService vpnPayService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _claimsService = claimsService;
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _vpnPayService = vpnPayService;
        }

        public async Task<OrderDTO> NewPurchaseAsync(VnpayOrderInfo orderInfo)
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
                var url = _vpnPayService.CreateLink(orderInfo);
                var result = _mapper.Map<OrderDTO>(newOrder);
                result.Note = url;
                return result;
            }
            else
            {
                throw new Exception("400 - Adding proccess has been failed");
            }
        }

        public async Task<List<OrderDTO>> GetOrdersAsync()
        {
            return _mapper.Map<List<OrderDTO>>(await _unitOfWork.OrderRepository.GetAllAsync());
        }
    }
}