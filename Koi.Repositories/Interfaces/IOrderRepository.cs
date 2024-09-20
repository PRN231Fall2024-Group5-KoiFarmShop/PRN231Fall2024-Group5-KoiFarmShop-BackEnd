using Koi.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<OrderDetail>> CreateOrderWithOrderDetails(Order order, List<KoiFish> purchaseFishes);

        Task<bool> DeleteOrder(int orderId);

        Task<List<Order>> GetAllOrdersAsync();

        Task<Order> GetOrdersById(int orderId);

        Task<List<Order>> GetOrdersByUserId(int userId);

        Task<Order> UpdateOrderStatus(int orderId, string status);
    }
}