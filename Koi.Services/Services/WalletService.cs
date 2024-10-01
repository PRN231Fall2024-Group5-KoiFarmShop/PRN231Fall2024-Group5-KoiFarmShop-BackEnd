﻿using AutoMapper;
using Azure.Core;
using Google.Apis.Drive.v3.Data;
using Koi.BusinessObjects;
using Koi.DTOs.Enums;
using Koi.DTOs.PaymentDTOs;
using Koi.DTOs.TransactionDTOs;
using Koi.DTOs.WalletDTOs;
using Koi.Repositories.Helper;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICurrentTime _currentTime;

        public WalletService(IUnitOfWork unitOfWork, IMapper mapper, IOrderService orderService, IVnPayService vnPayService, ICurrentTime currentTime)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderService = orderService;
            _vnPayService = vnPayService;
            _currentTime = currentTime;
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

                existingWallet = await _unitOfWork.WalletRepository.CreateAsync(newWallet);
                if (await _unitOfWork.SaveChangeAsync() <= 0)
                {
                    throw new Exception("400 - Adding proccess has been failed");
                }
            }

            var newWalletTransaction = new WalletTransaction
            {
                // UserId = existingWallet.UserId,
                TransactionType = "DEPOSIT",
                PaymentMethod = "VNPAY",
                Amount = amount,
                BalanceBefore = existingWallet.Balance,
                BalanceAfter = existingWallet.Balance + amount,
                TransactionDate = _currentTime.GetCurrentTime(),
                TransactionStatus = TransactionStatusEnums.PENDING.ToString(),
                Note = ""
            };

            var vnpayOrderInfo = new VnpayOrderInfo
            {
                Amount = amount,
                Description = ""
            };

            newWalletTransaction = await _unitOfWork.TransactionRepository.AddWalletTransaction(newWalletTransaction);
            if (await _unitOfWork.SaveChangeAsync() <= 0)
            {
                throw new Exception("400 - Adding proccess has been failed");
            }

            vnpayOrderInfo.CommonId = newWalletTransaction.Id;
            var payUrl = _vnPayService.CreateLink(vnpayOrderInfo);
            var result = new DepositResponseDTO
            {
                Transaction = _mapper.Map<WalletTransactionDTO>(newWalletTransaction),
                PayUrl = payUrl
            };

            return result;
        }

        public async Task<WalletDTO> GetWalletByUserId(int userId)
        {
            var wallet = await _unitOfWork.WalletRepository.GetWalletByUserId(userId);
            var result = _mapper.Map<WalletDTO>(wallet);
            return result;
        }

        public async Task<List<WalletDTO>> GetAllWallets()
        {
            var wallets = await _unitOfWork.WalletRepository.GetAllWalletsAsync();
            var result = _mapper.Map<List<WalletDTO>>(wallets);
            return result;
        }

        public async Task<WalletTransactionDTO> UpdateBalanceWallet(IQueryCollection query)
        {
            var response = _vnPayService.GetFullResponseData(query);
            var existingTransaction = await _unitOfWork.TransactionRepository.GetWalletTransactionsById(int.Parse(response.OrderId));
            if (existingTransaction == null)
            {
                throw new Exception("This transaction is not existing please check again");
            }

            //var user = await _unitOfWork.UserRepository.GetCurrentUserAsync();
            //if (user == null)
            //{
            //    throw new Exception("401 - User not existing");
            //}

            //if (existingOrder.UserId != user.Id)
            //{
            //    throw new Exception("You are not owner of this order");
            //}

            if (!response.Success)
            {
                existingTransaction.TransactionStatus = TransactionStatusEnums.FAILED.ToString();
            }
            else
            {
                var userWallet = await _unitOfWork.WalletRepository.GetWalletByUserId(2);
                existingTransaction.TransactionStatus = TransactionStatusEnums.COMPLETED.ToString();
                userWallet.Balance += existingTransaction.Amount;
            }
            if (await _unitOfWork.SaveChangeAsync() <= 0)
            {
                throw new Exception("400 - Adding transaction proccess has been failed");
            }
            return _mapper.Map<WalletTransactionDTO>(existingTransaction);
        }

        public async Task<WalletTransactionDTO> GetWalletTransactionById(int transactionId)
        {
            var transaction = await _unitOfWork.TransactionRepository.GetWalletTransactionsById(transactionId);
            var result = _mapper.Map<WalletTransactionDTO>(transaction);
            return result;
        }

        public async Task<List<WalletTransactionDTO>> GetTransactionsByOrderId(int orderId)
        {
            var transactions = await _unitOfWork.TransactionRepository.GetWalletTransactionsByOrderId(orderId);
            var result = _mapper.Map<List<WalletTransactionDTO>>(transactions);
            return result;
        }

        public async Task<List<WalletTransactionDTO>> GetWalletTransactionsByUserId(int userId)
        {
            var transactions = await _unitOfWork.TransactionRepository.GetWalletTransactionsByUserId(userId);
            var result = _mapper.Map<List<WalletTransactionDTO>>(transactions);
            return result;
        }

        public async Task<DepositResponseDTO> CompletePending(int transactionId)
        {
            //var user = await _unitOfWork.UserRepository.GetCurrentUserAsync();
            //if (user == null)
            //{
            //    throw new Exception("401 - User not existing");
            //}
            var existingWalletTransaction = await _unitOfWork.TransactionRepository.GetWalletTransactionsById(transactionId);
            if (existingWalletTransaction == null)
            {
                return null;
            }

            var existingWallet = await _unitOfWork.WalletRepository.GetWalletByUserId(2);
            if (existingWallet == null)
            {
                throw new Exception("404 - This user don't have wallet");
            }
            if (existingWalletTransaction.TransactionStatus == "COMPLETED")
            {
                throw new Exception("400 - This order have been completed");
            }

            var vnpayOrderInfo = new VnpayOrderInfo
            {
                CommonId = existingWalletTransaction.Id,
                Amount = existingWalletTransaction.Amount,
                Description = ""
            };
            var payUrl = _vnPayService.CreateLink(vnpayOrderInfo);
            var result = new DepositResponseDTO
            {
                Transaction = _mapper.Map<WalletTransactionDTO>(existingWallet),
                PayUrl = payUrl
            };

            return result;
        }

        // Purchase method use wallet amount to pay order
        public async Task<OrderDTO> CheckOut(PurchaseDTO purchaseDTO)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetCurrentUserAsync();
                if (user == null)
                {
                    throw new Exception("401 - User are signed out");
                }

                var fishes = new List<KoiFish>();
                long? totalAmount = 0;
                foreach (var fish in purchaseDTO.FishIds)
                {
                    var koiFish = await _unitOfWork.KoiFishRepository.GetByIdAsync(fish);
                    if (koiFish != null)
                    {
                        fishes.Add(koiFish);
                        totalAmount += koiFish.Price;
                    }
                    else
                    {
                        throw new Exception("404 - This koi fish not found id:" + fish);
                    }
                }

                var newOrder = new Order
                {
                    UserId = user.Id,
                    TotalAmount = totalAmount.Value,
                    OrderStatus = OrderStatusEnums.PENDING.ToString(),
                    ShippingAddress = purchaseDTO.ShippingAddress,
                    PaymentMethod = "VNPAY",
                    Note = purchaseDTO.ShippingAddress,
                };

                var personalWallet = await _unitOfWork.WalletRepository.GetWalletByUserId(user.Id);
                var wallet = user.Wallet;
                if (personalWallet == null)
                {
                    throw new Exception("Personal Wallet not found+");
                }

                // Check balance
                if (personalWallet.Balance < newOrder.TotalAmount)
                {
                    throw new Exception("You dont have enough money to purchase this order");
                }
                newOrder = await _unitOfWork.OrderRepository.AddAsync(newOrder);

                if (await _unitOfWork.SaveChangeAsync() <= 0)
                {
                    throw new Exception("400 - Adding order proccess has been failed");
                }

                var orderDetails = await _unitOfWork.OrderRepository.CreateOrderWithOrderDetails(newOrder, fishes);
                //if (await _unitOfWork.SaveChangeAsync() <= 0)
                //{
                //    throw new Exception("400 - Adding order details proccess has been failed");
                //}

                // Send notification
                //var notification = new Notification
                //{
                //    Title = "Purchase order " + orderId,
                //    Body = "Amount: " + order.TotalAmount,
                //    UserId = userId,
                //    Url = "/profile/orders",
                //    Sender = "System"
                //};

                //await _notificationService.PushNotification(notification);

                // Purchase
                var transation = await PurchaseItem(user.Id, newOrder);

                //Notification to event order
                //var notificationToOrganizer = new Notification
                //{
                //    Title = "One person buy your package" + orderId,
                //    Body = "Amount: " + order.TotalAmount,
                //    UserId = eventModel.UserId,
                //    Url = "/dashboard/my-events/" + eventModel.Id + "/orders",
                //    Sender = "System"
                //};
                //await _notificationService.PushNotification(notificationToOrganizer);

                var result = _mapper.Map<OrderDTO>(newOrder);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Purchase item and deduct money from wallet
        public async Task<WalletTransactionDTO> PurchaseItem(int userId, Order createdOrder)
        {
            //Get order
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(createdOrder.Id);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            if (order.UserId != userId)
            {
                throw new Exception("Order not belong to user");
            }
            if (order.OrderStatus == OrderStatusEnums.COMPLETED.ToString())
            {
                throw new Exception("This order has been paid already");
            }
            if (order.OrderStatus == OrderStatusEnums.CANCELLED.ToString())
            {
                throw new Exception("This order has been cancelled");
            }

            var existingWallet = await _unitOfWork.WalletRepository.GetWalletByUserId(userId);
            if (existingWallet == null)
            {
                throw new Exception("Wallet not found");
            }

            if (existingWallet.Balance < order.TotalAmount)
            {
                throw new Exception("Balance is not enough");
            }

            try
            {
                //Create new transaction
                var newWalletTransaction = new WalletTransaction
                {
                    //UserId = existingWallet.UserId,
                    OrderId = order.Id,
                    TransactionType = "PURCHASE",
                    TransactionStatus = TransactionStatusEnums.COMPLETED.ToString(),
                    PaymentMethod = "WALLET",
                    Amount = order.TotalAmount,
                    BalanceBefore = existingWallet.Balance,
                    BalanceAfter = existingWallet.Balance - order.TotalAmount,
                    TransactionDate = _currentTime.GetCurrentTime(),
                    Note = ""
                };

                newWalletTransaction = await _unitOfWork.TransactionRepository.AddAsync(newWalletTransaction);
                //Update wallet balance
                existingWallet.Balance -= order.TotalAmount;
                if (existingWallet.Balance < 0)
                {
                    throw new Exception("PURCHASE: Balance is not enough");
                }

                //update order status
                order.OrderStatus = OrderStatusEnums.COMPLETED.ToString();
                await _unitOfWork.WalletRepository.UpdateWallet(existingWallet);
                await _unitOfWork.OrderRepository.Update(order);

                if (await _unitOfWork.SaveChangeAsync() <= 0)
                {
                    throw new Exception("400 - Adding proccess has been failed");
                }

                var result = _mapper.Map<WalletTransactionDTO>(newWalletTransaction);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}