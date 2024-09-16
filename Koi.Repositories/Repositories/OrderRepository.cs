using Koi.BusinessObjects;
using Koi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Koi.Repositories.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly KoiFarmShopDbContext _dbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public OrderRepository(KoiFarmShopDbContext context, ICurrentTime timeService, IClaimsService claims) : base(context, timeService, claims)
        {
            _dbContext = context;
            _timeService = timeService;
            _claimsService = claims;
        }

        // Lấy danh sách đơn hàng của người dùng theo UserId
        public async Task<List<Order>> GetOrdersByUserId(int userId)
        {
            return await _dbContext.Orders
                                   .Where(o => o.UserId == userId)
                                   .ToListAsync();
        }

        public async Task<Order> GetOrdersById(int orderId)
        {
            return await _dbContext.Orders.FindAsync(orderId);
        }

        // Cập nhật trạng thái của đơn hàng
        public async Task<Order> UpdateOrderStatus(int orderId, string status)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return null; // Hoặc ném ngoại lệ nếu cần thiết

            order.OrderStatus = status;
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();

            return order;
        }

        // Xóa đơn hàng
        public async Task<bool> DeleteOrder(int orderId)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return false;

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        // Lấy tất cả đơn hàng
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _dbContext.Orders.ToListAsync();
        }
    }
}