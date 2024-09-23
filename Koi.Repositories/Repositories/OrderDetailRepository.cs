using Koi.BusinessObjects;
using Koi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Koi.Repositories.Repositories
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly KoiFarmShopDbContext _dbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public OrderDetailRepository(KoiFarmShopDbContext context, ICurrentTime timeService, IClaimsService claims) : base(context, timeService, claims)

        {
            _dbContext = context;
            _timeService = timeService;
            _claimsService = claims;
        }

        public async Task<OrderDetail> ChangeToCanceled(int id)
        {

            var tar = await _dbContext.OrderDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (tar == null) throw new Exception("404 - Not Found OrderDetail");
            tar.Status = "CANCELED";
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return tar;
        }

        public async Task<OrderDetail> ChangeToCompleted(int id)
        {
            var tar = await _dbContext.OrderDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (tar == null) throw new Exception("404 - Not Found OrderDetail");
            tar.Status = "COMPLETED";
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return tar;
        }

        public async Task<OrderDetail> ChangeToConsigned(int id)
        {
            var tar = await _dbContext.OrderDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (tar == null) throw new Exception("404 - Not Found OrderDetail");
            tar.Status = "CONSIGNED";
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return tar;
        }

        public async Task<OrderDetail> ChangeToShipping(int id)
        {
            var tar = await _dbContext.OrderDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (tar == null) throw new Exception("404 - Not Found OrderDetail");
            tar.Status = "SHIPPING";
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return tar;
        }
    }
}
