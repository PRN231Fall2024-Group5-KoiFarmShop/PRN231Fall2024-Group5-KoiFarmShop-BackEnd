using Koi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KoiFarmShopDbContext _context;

        public UnitOfWork(KoiFarmShopDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangeAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}