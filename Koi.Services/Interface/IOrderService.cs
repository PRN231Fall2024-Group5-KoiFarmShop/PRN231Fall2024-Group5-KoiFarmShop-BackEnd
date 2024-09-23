using Koi.DTOs.PaymentDTOs;

namespace Koi.Services.Interface
{
    public interface IOrderService
    {
        Task<OrderDTO> GetOrderByIdAsync(int orderId);
        Task<List<OrderDTO>> GetOrdersAsync();
        Task<List<OrderDTO>> GetOrdersByUserIdAsync(int userId);
        Task<OrderDTO> NewOrderAsync(VnpayOrderInfo orderInfo);
        Task<OrderDTO> CancelOrderAsync(int id);
    }
}