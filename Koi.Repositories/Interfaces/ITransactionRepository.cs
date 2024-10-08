﻿using Koi.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<WalletTransaction>
    {
        Task<WalletTransaction> AddWalletTransaction(WalletTransaction walletTransaction);

        Task<List<WalletTransaction>> GetWalletTransactionsByOrderId(int orderId);

        Task<WalletTransaction> GetWalletTransactionsById(int id);

        Task<List<WalletTransaction>> GetWalletTransactionsByUserId(int userId);
    }
}