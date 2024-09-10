﻿namespace Koi.Repositories.Entities
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; }
        public string? Body { get; set; }
        public string? Url { get; set; }
        public int? ReceiverId { get; set; }
        public string Type { get; set; } // USER, GROUP, ALL
        public bool IsRead { get; set; } = false;
    }
}