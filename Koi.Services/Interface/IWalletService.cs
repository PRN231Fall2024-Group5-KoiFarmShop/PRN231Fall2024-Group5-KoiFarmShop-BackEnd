using Koi.BusinessObjects;
using Koi.DTOs.PaymentDTOs;
using Koi.DTOs.TransactionDTOs;
using Koi.DTOs.WalletDTOs;
using Microsoft.AspNetCore.Http;

namespace Koi.Services.Interface
{
    public interface IWalletService
    {
        Task<DepositResponseDTO> CompletePending(int orderId);

        Task<DepositResponseDTO> Deposit(long amount);
        Task<DepositResponseDTO> DepositByPayOS(int amount);

        Task<List<WalletDTO>> GetAllWallets();

        Task<WalletTransactionDTO> GetWalletTransactionById(int transactionId);

        Task<List<WalletTransactionDTO>> GetTransactionsByOrderId(int orderId);

        Task<WalletDTO> GetWalletByUserId(int userId);

        Task<WalletTransactionDTO> UpdateBalanceWallet(IQueryCollection query);
        Task<List<WalletTransactionDTO>> GetWalletTransactionsByUserId(int userId);
        Task<OrderDTO> CheckOut(PurchaseDTO purchaseDTO);
        Task<WalletTransactionDTO> PurchaseItem(int userId, Order order);
    }
}