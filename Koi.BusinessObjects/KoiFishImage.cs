﻿namespace Koi.BusinessObjects
{
    public class KoiFishImage : BaseEntity
    {
        public int KoiFishId { get; set; }
        public string? Name { get; set; }
        public string ImageUrl { get; set; }
        public virtual KoiFish KoiFish { get; set; }
    }
}