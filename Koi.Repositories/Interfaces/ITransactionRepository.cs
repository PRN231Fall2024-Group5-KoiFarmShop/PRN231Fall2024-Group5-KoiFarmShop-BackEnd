using Koi.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<WalletTransaction> AddWalletTransaction(WalletTransaction walletTransaction);
        Task<List<Transaction>> GetTransactionsByOrderId(int orderId);
        Task<WalletTransaction> GetWalletTransactionsById(int id);
    }
}