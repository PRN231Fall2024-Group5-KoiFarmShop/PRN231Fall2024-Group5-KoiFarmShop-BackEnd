using Koi.BusinessObjects;
using Koi.DTOs.Enums;
using Koi.DTOs.WalletDTOs;
using Koi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Koi.Repositories.Repositories
{
    public class WithdrawnRequestRepository : GenericRepository<WithdrawnRequest>, IWithdrawnRequestRepository
    {
        private readonly KoiFarmShopDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;
        public WithdrawnRequestRepository(KoiFarmShopDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        // Check Wallet balance before creating a request
        public async Task<WithdrawnRequest> CreateARequest(WithdrawnRequest request)
        {
            if (_claimsService.GetCurrentUserId == 0)
            {
                throw new Exception("User not found.");
            }
            if (request.Amount <= 0)
            {
                throw new Exception("Invalid amount.");
            }

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => (w.UserId == _claimsService.GetCurrentUserId));

            if (wallet == null || wallet.Balance < request.Amount)
            {
                throw new Exception("Insufficient funds in the wallet.");
            }
            // Deduct amount from wallet
            wallet.Balance -= request.Amount;

            request.Status = "Pending";
            request.CreatedAt = _timeService.GetCurrentTime();
            request.CreatedBy = _claimsService.GetCurrentUserId;
            request.UserId = _claimsService.GetCurrentUserId;

            _context.WithdrawnRequests.Add(request);
            await _context.SaveChangesAsync();

            return request;
        }

        // Approve the request, subtract the amount from the wallet, and create a transaction
        public async Task<WithdrawnRequest> ApproveRequest(int requestId, string imageUrl)
        {
            var request = await _context.WithdrawnRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (request == null)
            {
                throw new Exception("Request not found.");
            }

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == request.UserId);
            //get user wallet
            // Update request status
            request.Status = "Success";
            request.ImageUrl = imageUrl;
            request.ModifiedAt = _timeService.GetCurrentTime();
            request.ModifiedBy = _claimsService.GetCurrentUserId;

            // Create a withdrawal transaction
            var transaction = new WalletTransaction
            {
                Amount = request.Amount,
                TransactionDate = _timeService.GetCurrentTime(),
                TransactionStatus = TransactionStatusEnums.COMPLETED.ToString(),
                CreatedAt = _timeService.GetCurrentTime(),
                CreatedBy = _claimsService.GetCurrentUserId,
                Note = "Withdrawal " + request.Amount,
                WalletId = request.UserId,
                BalanceBefore = wallet.Balance,
                BalanceAfter = wallet.Balance - request.Amount,
                TransactionType = "Withdrawal",
                PaymentMethod = "Bank Transfer"
            };

            _context.WalletTransactions.Add(transaction);

            await _context.SaveChangesAsync();
            return request;
        }

        // Reject a request
        public async Task<WithdrawnRequest> RejectRequest(int requestId)
        {
            var request = await _context.WithdrawnRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (request == null)
            {
                throw new Exception("Request not found.");
            }

            request.Status = "Rejected";
            request.ModifiedAt = _timeService.GetCurrentTime();
            request.ModifiedBy = _claimsService.GetCurrentUserId;

            //Add the amount back to the wallet
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == request.UserId);
            wallet.Balance += request.Amount;

            await _context.SaveChangesAsync();
            return request;
        }

        // Get all requests
        public async Task<List<WithdrawnRequest>> GetAllRequest()
        {
            return await _context.WithdrawnRequests.Include(r => r.User).ToListAsync();
        }

        public Task<List<WithdrawnRequest>> GetRequestByUserId()
        {
            return _context.WithdrawnRequests.Where(r => r.UserId == _claimsService.GetCurrentUserId).ToListAsync();
        }

        public async Task<DashboardOrderStatisticsDto> Analyst(DateTime startDate, DateTime endDate)
        {
            // Filter orders within the date range
            var orders = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .ToListAsync();

            // Group orders by status and count them
            var orderStatusCounts = orders
                .GroupBy(o => o.OrderStatus)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToDictionary(x => x.Status, x => x.Count);

            // Count the total number of orders
            int totalOrders = orders.Count;

            // Calculate total revenue within the date range
            long totalRevenue = orders.Sum(o => o.TotalAmount);

            // Prepare the result object to return statistics
            var result = new DashboardOrderStatisticsDto
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                OrdersByStatus = orderStatusCounts,
                StartDate = startDate,
                EndDate = endDate
            };

            return result;
        }

    }
}
