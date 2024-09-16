using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.PaymentDTOs
{
    public class PaymentResponseModel
    {
        public string OrderDescription { get; set; }
        public string TransactionToken { get; set; }
        public string OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionNo { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }

        public decimal? AmountOfMoney { get; set; }

        public string BanKTranNo { get; set; }

        public DateTime PayDate { get; set; }
    }
}