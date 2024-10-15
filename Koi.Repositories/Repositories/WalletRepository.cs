using Koi.BusinessObjects;
using Koi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Koi.Repositories.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly KoiFarmShopDbContext _dbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public WalletRepository(KoiFarmShopDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService)
        {
            _dbContext = dbContext;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        // Tạo mới ví
        public async Task<Wallet> CreateAsync(Wallet wallet)
        {
            wallet.CreatedAt = _timeService.GetCurrentTime();
            var result = await _dbContext.Wallets.AddAsync(wallet);

            return result.Entity;
        }

        // Cập nhật ví
        public async Task<Wallet> UpdateWallet(Wallet updateWallet)
        {
            var existingWallet = await _dbContext.Wallets.FirstOrDefaultAsync(w => w.UserId == updateWallet.UserId);

            if (existingWallet == null)
                return null; // Hoặc ném ngoại lệ

            existingWallet.Balance = updateWallet.Balance;
            existingWallet.ModifiedAt = _timeService.GetCurrentTime();
            existingWallet.Status = updateWallet.Status;

            _dbContext.Wallets.Update(existingWallet);

            return existingWallet;
        }

        // Lấy ví theo UserId
        public async Task<Wallet> GetWalletByUserId(int userId)
        {
            return await _dbContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        }

        // Xóa ví
        public async Task<bool> DeleteWallet(int userId)
        {
            var wallet = await _dbContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
                return false;

            wallet.IsDeleted = true;

            return true;
        }

        // Lấy tất cả ví
        public async Task<List<Wallet>> GetAllWalletsAsync()
        {
            return await _dbContext.Wallets.ToListAsync();
        }

        public async Task<Wallet> GetAllWalletByIdAsync(int id)
        {
            return await _dbContext.Wallets.FindAsync(id);
        }
    }
}