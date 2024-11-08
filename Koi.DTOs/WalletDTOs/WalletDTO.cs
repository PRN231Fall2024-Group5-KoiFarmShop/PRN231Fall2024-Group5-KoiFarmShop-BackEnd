namespace Koi.DTOs.WalletDTOs
{
    public class WalletDTO
    {
        public int UserId { get; set; } // Primary key and foreign key pointing to User

        public Int64 Balance { get; set; }
        public int LoyaltyPoints { get; set; }
        public string Status { get; set; }
    }

    public class DashboardOrderStatisticsDto
    {
        public int TotalOrders { get; set; }
        public long TotalRevenue { get; set; }
        public Dictionary<string, int> OrdersByStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}