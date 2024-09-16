using Koi.DTOs.PaymentDTOs;
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
        Task<DepositResponseDTO> Deposit(long amount);
        Task<TransactionDTO> GetTransactionById(int transactionId);
        Task<List<TransactionDTO>> GetTransactionsByOrderId(int orderId);
        Task<TransactionDTO> UpdateBalanceWallet(IQueryCollection query);
    }
}