using Koi.BusinessObjects;
using Koi.DTOs.Enums;
using Koi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
                                    .Include(x => x.User).Include(x => x.OrderDetails).ThenInclude(x => x.KoiFish).ThenInclude(x => x.ConsignmentForNurtures)

                                   .Where(o => o.UserId == userId)
                                   .ToListAsync();
        }

        public async Task<Order> GetOrdersById(int orderId)
        {
            return await _dbContext.Orders.Include(x => x.User).Include(x => x.OrderDetails).ThenInclude(x => x.KoiFish).ThenInclude(x => x.ConsignmentForNurtures).FirstOrDefaultAsync(x => x.Id == orderId);
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
            var order = await GetOrdersById(orderId);

            if (order == null)
                return false;

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        // Lấy tất cả đơn hàng
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _dbContext.Orders.Include(x => x.OrderDetails).ThenInclude(x => x.ConsignmentForNurture).ThenInclude(x => x.KoiFish).Include(x => x.User).ToListAsync();
        }

        public async Task<List<OrderDetail>> CreateOrderWithOrderDetails(Order order, List<KoiFish> purchaseFishes)
        {
            try
            {
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                foreach (var fish in purchaseFishes)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        KoiFishId = fish.Id,
                        // SubTotal = (int)fish.Price,
                        Price = fish.Price,
                        Status = OrderStatusEnums.PENDING.ToString(),
                    };
                    if (fish.IsConsigned.Value && fish.ConsignmentForNurtures.Any())
                    {
                        var existingConsignment = fish.ConsignmentForNurtures.ToList();
                        orderDetail.ConsignmentForNurtureId = existingConsignment.First().Id;
                        orderDetail.ConsignmentCost = existingConsignment.First().ProjectedCost;
                    }
                    order.OrderDetails = [];
                    order.OrderDetails.Add(orderDetail);
                    orderDetails.Add(orderDetail);
                    fish.OwnerId = _claimsService.GetCurrentUserId;
                }

                _dbContext.Entry(order).State = EntityState.Modified;
                await _dbContext.OrderDetails.AddRangeAsync(orderDetails);
                // await _dbContext.SaveChangesAsync();

                return orderDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}