﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Koi.BusinessObjects
{
    public class OrderDetail : BaseEntity
    {
        public int OrderId { get; set; }
        public int KoiFishId { get; set; }

        public int? ConsignmentForNurtureId { get; set; }

        public long Price { get; set; }
        public string? Status { get; set; }
        public string? ShippingStatus { get; set; }
        public string? NurtureStatus { get; set; }

        //navigation
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        public virtual KoiFish KoiFish { get; set; }
        public virtual ICollection<OrderDetailFeedback> OrderDetailFeedbacks { get; set; }
        public virtual ConsignmentForNurture? ConsignmentForNurture { get; set; }
    }
}