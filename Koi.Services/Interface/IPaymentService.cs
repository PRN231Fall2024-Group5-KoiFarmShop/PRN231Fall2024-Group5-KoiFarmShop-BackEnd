using Koi.DTOs.PaymentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Services.Interface
{
    public interface IPaymentService
    {
        Task<List<OrderDTO>> GetOrdersAsync();

        Task<OrderDTO> NewPurchaseAsync(VnpayOrderInfo orderInfo);
    }
}