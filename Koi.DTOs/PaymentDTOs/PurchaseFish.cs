﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.PaymentDTOs
{
    public class PurchaseFish
    {
        public int FishId { get; set; }
        public bool IsNuture { get; set; } = false;
        public int DietId { get; set; } // Foreign Key to Diet

        public DateTime StartDate { get; set; }  // Ngày bắt đầu chăm sóc
        public DateTime EndDate { get; set; }  // Ngày kết thúc chăm sóc
        public string? Note { get; set; }
    }
}