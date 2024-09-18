using Koi.DTOs.TransactionDTOs;
using Koi.DTOs.WalletDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Services.Interface
{
    public interface IWalletService
    {
        Task<DepositResponseDTO> CompletePending(int orderId);
        Task<DepositResponseDTO> Deposit(long amount);

        Task<List<WalletDTO>> GetAllWallets();

        Task<TransactionDTO> GetTransactionById(int transactionId);

        Task<List<TransactionDTO>> GetTransactionsByOrderId(int orderId);

        Task<WalletDTO> GetWalletByUserId(int userId);

        Task<TransactionDTO> UpdateBalanceWallet(IQueryCollection query);
    }
}