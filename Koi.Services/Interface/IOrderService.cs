using Koi.DTOs.PaymentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Services.Interface
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> GetOrdersAsync();

        Task<OrderDTO> NewOrderAsync(VnpayOrderInfo orderInfo);
    }
}