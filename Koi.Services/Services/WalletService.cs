using AutoMapper;
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
            vnpayOrderInfo.CommonId = newOrder.Id;
            var payUrl = _vnPayService.CreateLink(vnpayOrderInfo);
            var result = new DepositResponseDTO
            {
                Order = newOrder,
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

        public async Task<TransactionDTO> UpdateBalanceWallet(IQueryCollection query)
        {
            var response = _vnPayService.GetFullResponseData(query);
            var existingOrder = await _unitOfWork.OrderRepository.GetByIdAsync(int.Parse(response.OrderId));
            if (existingOrder == null)
            {
                throw new Exception("This order is not existing please check again");
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
                var failTransaction = new Transaction
                {
                    UserId = existingOrder.UserId,
                    OrderId = existingOrder.Id,
                    PaymentMethod = "VNPAY",
                    TransactionDate = _currentTime.GetCurrentTime(),
                    TransactionStatus = TransactionStatusEnums.FAILED.ToString()
                };

                existingOrder.OrderStatus = "CANCELLED";
                if (await _unitOfWork.SaveChangeAsync() <= 0)
                {
                    throw new Exception("400 - Adding proccess has been failed");
                }
                return _mapper.Map<TransactionDTO>(failTransaction);
            }
            else
            {
            }
            var newTransaction = new Transaction
            {
                UserId = existingOrder.UserId,
                OrderId = existingOrder.Id,
                PaymentMethod = "VNPAY",
                TransactionDate = _currentTime.GetCurrentTime(),
                TransactionStatus = TransactionStatusEnums.FAILED.ToString(),
                Amount = (long)response.AmountOfMoney
            };
            newTransaction = await _unitOfWork.TransactionRepository.AddAsync(newTransaction);
            var userWallet = await _unitOfWork.WalletRepository.GetWalletByUserId(existingOrder.UserId);
            existingOrder.OrderStatus = "COMPLETED";
            userWallet.Balance += newTransaction.Amount;
            if (await _unitOfWork.SaveChangeAsync() <= 0)
            {
                throw new Exception("400 - Adding proccess has been failed");
            }
            return _mapper.Map<TransactionDTO>(newTransaction);
        }

        public async Task<TransactionDTO> GetTransactionById(int transactionId)
        {
            var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(transactionId);
            var result = _mapper.Map<TransactionDTO>(transaction);
            return result;
        }

        public async Task<List<TransactionDTO>> GetTransactionsByOrderId(int orderId)
        {
            var transactions = await _unitOfWork.TransactionRepository.GetTransactionsByOrderId(orderId);
            var result = _mapper.Map<List<TransactionDTO>>(transactions);
            return result;
        }

        public async Task<DepositResponseDTO> CompletePending(int orderId)
        {
            //var user = await _unitOfWork.UserRepository.GetCurrentUserAsync();
            //if (user == null)
            //{
            //    throw new Exception("401 - User not existing");
            //}
            var existingOrder = await _orderService.GetOrderByIdAsync(orderId);
            if (existingOrder == null)
            {
                return null;
            }

            var existingWallet = await _unitOfWork.WalletRepository.GetWalletByUserId(2);
            if (existingWallet == null)
            {
                throw new Exception("404 - This user don't have wallet");
            }
            if (existingOrder.OrderStatus == "COMPLETED")
            {
                throw new Exception("400 - This order have been completed");
            }

            var vnpayOrderInfo = new VnpayOrderInfo
            {
                CommonId = existingOrder.Id,
                Amount = existingOrder.TotalAmount,
                Description = ""
            };
            var payUrl = _vnPayService.CreateLink(vnpayOrderInfo);
            var result = new DepositResponseDTO
            {
                Order = existingOrder,
                PayUrl = payUrl
            };

            return result;
        }

        // Purchase method use wallet amount to pay order
        //public async Task<OrderDTO> CheckOut(PurchaseDTO purchaseDTO)
        //{
        //    var user = await _unitOfWork.UserRepository.GetCurrentUserAsync();
        //    if (user == null)
        //    {
        //        throw new Exception("401 - User not existing");
        //    }

        //    var fishes = new List<KoiFish>();
        //    long? totalAmount = 0;
        //    foreach (var fish in purchaseDTO.FishIds)
        //    {
        //        var koiFish = await _unitOfWork.KoiFishRepository.GetByIdAsync(fish);
        //        if (koiFish != null)
        //        {
        //            fishes.Add(koiFish);
        //            totalAmount += koiFish.Price;
        //        }
        //        else
        //        {
        //            throw new Exception("404 - This koi fish not found id:" + fish);
        //        }
        //    }

        //    var newOrder = new Order
        //    {
        //        UserId = user.Id,
        //        TotalAmount = totalAmount.Value,
        //        OrderStatus = EventOrderStatusEnums.PENDING.ToString(),
        //        ShippingAddress = purchaseDTO.ShippingAddress,
        //        PaymentMethod = "VNPAY",
        //        Note = purchaseDTO.ShippingAddress
        //    };

        //    var personalWallet = await _unitOfWork.WalletRepository.GetWalletByUserId(user.Id);
        //    if (personalWallet == null)
        //    {
        //        throw new Exception("Peronsal Wallet not found+");
        //    }

        //    // Check balance
        //    if (personalWallet.Balance < newOrder.TotalAmount)
        //    {
        //        throw new Exception("You dont have enough money to purchase this order");
        //    }

        //    // Send notification
        //    //var notification = new Notification
        //    //{
        //    //    Title = "Purchase order " + orderId,
        //    //    Body = "Amount: " + order.TotalAmount,
        //    //    UserId = userId,
        //    //    Url = "/profile/orders",
        //    //    Sender = "System"
        //    //};

        //    //await _notificationService.PushNotification(notification);

        //    // Purchase
        //    var transation = await _unitOfWork.WalletRepository.PurchaseItem(userId, orderId);

        //    //Increase money of event owner
        //    var eventModel = await _unitOfWork.EventRepository.GetByIdAsync(order.EventId);
        //    var eventOwnerWallet = await _unitOfWork.WalletRepository.GetWalletByUserIdAndType(eventModel.UserId, WalletTypeEnums.PERSONAL);
        //    eventOwnerWallet.Balance += order.TotalAmount;
        //    await _unitOfWork.WalletRepository.Update(eventOwnerWallet);
        //    await _unitOfWork.SaveChangeAsync();

        //    //Notification to event order
        //    //var notificationToOrganizer = new Notification
        //    //{
        //    //    Title = "One person buy your package" + orderId,
        //    //    Body = "Amount: " + order.TotalAmount,
        //    //    UserId = eventModel.UserId,
        //    //    Url = "/dashboard/my-events/" + eventModel.Id + "/orders",
        //    //    Sender = "System"
        //    //};
        //    //await _notificationService.PushNotification(notificationToOrganizer);

        //    var result = _mapper.Map<TransactionResponsesDTO>(transation);
        //    return result;
        //}

        //// Purchase item and deduct money from wallet
        //public async Task<Transaction> PurchaseItem(Guid userId, Order createdOrder)
        //{
        //    //Get order
        //    var order = await _unitOfWork.OrderRepository.GetByIdAsync(createdOrder.Id);
        //    if (order == null)
        //    {
        //        throw new Exception("Order not found");
        //    }
        //    if (order.UserId != userId)
        //    {
        //        throw new Exception("Order not belong to user");
        //    }
        //    if (order.Status == EventOrderStatusEnums.PAID.ToString())
        //    {
        //        throw new Exception("This order has been paid already");
        //    }
        //    if (order.Status == EventOrderStatusEnums.CANCELLED.ToString())
        //    {
        //        throw new Exception("This order has been cancelled");
        //    }

        //    var wallet = await GetListWalletByUserId(userId);
        //    var personalWallet = wallet.FirstOrDefault(x => x.WalletType.ToUpper() == WalletTypeEnums.PERSONAL.ToString().ToUpper());
        //    if (personalWallet == null)
        //    {
        //        throw new Exception("Wallet not found");
        //    }

        //    if (personalWallet.Balance < order.TotalAmount)
        //    {
        //        throw new Exception("Balance is not enough");
        //    }

        //    using (var txn = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            //Create new transaction
        //            var transaction = new Transaction
        //            {
        //                WalletId = personalWallet.Id,
        //                TransactionType = TransactionTypeEnums.PURCHASE.ToString(),
        //                Amount = order.TotalAmount,
        //                Description = "Purchase order with amount: " + order.TotalAmount,
        //                TransactionDate = _timeService.GetCurrentTime(),
        //                CreatedAt = _timeService.GetCurrentTime(),
        //                Status = TransactionStatusEnums.SUCCESS.ToString()
        //            };

        //            //add transaction to database
        //            await _context.Transactions.AddAsync(transaction);
        //            await _context.SaveChangesAsync();

        //            //Create new transaction detail
        //            var transactionDetail = new TransactionDetail
        //            {
        //                TransactionId = transaction.Id,
        //                EventOrderId = order.Id,
        //            };
        //            //add transaction detail to database
        //            await _context.TransactionDetails.AddAsync(transactionDetail);
        //            await _context.SaveChangesAsync();

        //            //Update wallet balance
        //            personalWallet.Balance -= order.TotalAmount;
        //            if (personalWallet.Balance < 0)
        //            {
        //                throw new Exception("PURCHASE: Balance is not enough");
        //            }
        //            _context.Entry(personalWallet).State = EntityState.Modified;

        //            //update order status
        //            order.Status = EventOrderStatusEnums.PAID.ToString();
        //            _context.Entry(order).State = EntityState.Modified;
        //            await _context.SaveChangesAsync();

        //            await txn.CommitAsync();
        //            return transaction;
        //        }
        //        catch (Exception ex)
        //        {
        //            await txn.RollbackAsync();
        //            throw ex;
        //        }
        //    }
        //}
    }
}