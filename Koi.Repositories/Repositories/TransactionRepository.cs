using Koi.BusinessObjects;
using Koi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Repositories
{
    public class TransactionRepository : GenericRepository<WalletTransaction>, ITransactionRepository
    {
        private readonly KoiFarmShopDbContext _dbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public TransactionRepository(KoiFarmShopDbContext context, ICurrentTime timeService, IClaimsService claims) : base(context, timeService, claims)

        {
            _dbContext = context;
            _timeService = timeService;
            _claimsService = claims;
        }

        public async Task<List<WalletTransaction>> GetWalletTransactionsByOrderId(int orderId)
        {
            return await _dbContext.WalletTransactions
                                   .Where(o => o.OrderId == orderId)
                                   .ToListAsync();
        }

        public async Task<WalletTransaction> AddWalletTransaction(WalletTransaction walletTransaction)
        {
            walletTransaction.CreatedAt = _timeService.GetCurrentTime();
            walletTransaction.CreatedBy = _claimsService.GetCurrentUserId;
            var result = await _dbContext.WalletTransactions.AddAsync(walletTransaction);
            return result.Entity;
        }

        public async Task<WalletTransaction> GetWalletTransactionsById(int id)
        {
            return await _dbContext.WalletTransactions.FindAsync(id);
        }

        public async Task<List<WalletTransaction>> GetWalletTransactionsByUserId(int userId)
        {
            return await _dbContext.WalletTransactions.Where(x => x.Wallet.UserId == userId).ToListAsync();
        }
    }
}