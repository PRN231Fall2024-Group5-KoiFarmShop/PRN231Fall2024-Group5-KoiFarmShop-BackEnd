using Koi.BusinessObjects;
using Koi.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet> CreateAsync(Wallet wallet);

        Task<bool> DeleteWallet(int userId);

        Task<List<Wallet>> GetAllWalletsAsync();

        Task<Wallet> GetWalletByUserId(int userId);

        Task<Wallet> UpdateWallet(Wallet updateWallet);
    }
}