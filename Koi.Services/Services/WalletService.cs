using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.PaymentDTOs;
using Koi.DTOs.WalletDTOs;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;
using ServiceLayer.Services.VnPayConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Services.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly IVnPayService _vnPayService;

        public WalletService(IUnitOfWork unitOfWork, IMapper mapper, IOrderService orderService, IVnPayService vnPayService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderService = orderService;
            _vnPayService = vnPayService;
        }

        // Deposit money to wallet
        public async Task<DepositResponseDTO> Deposit(long amount)
        {
            //var user = await _unitOfWork.UserRepository.GetCurrentUserAsync();
            //if (user == null)
            //{
            //    throw new Exception("401 - User not existing");
            //}
            var existingWallet = await _unitOfWork.WalletRepository.GetWalletByUserId(2);
            if (existingWallet == null)
            {
                var newWallet = new Wallet
                {
                    UserId = 2,
                    Balance = 0,
                    LoyaltyPoints = 0,
                    Status = ""
                };

                newWallet = await _unitOfWork.WalletRepository.CreateAsync(newWallet);
                if (await _unitOfWork.SaveChangeAsync() <= 0)
                {
                    throw new Exception("400 - Adding proccess has been failed");
                }
            }

            var vnpayOrderInfo = new VnpayOrderInfo
            {
                Amount = amount,
                Description = ""
            };

            var newOrder = await _orderService.NewOrderAsync(vnpayOrderInfo);
            vnpayOrderInfo.OrderId = newOrder.Id;
            var payUrl = _vnPayService.CreateLink(vnpayOrderInfo);
            var result = new DepositResponseDTO
            {
                Order = newOrder,
                PayUrl = payUrl
            };

            return result;
        }
    }
}