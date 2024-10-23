﻿using Koi.BusinessObjects;
using Koi.DTOs.Enums;
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
            if (wallet == null || wallet.Balance < request.Amount)
            {
                throw new Exception("Insufficient funds in the wallet.");
            }

            // Update request status
            request.Status = "Success";
            request.ImageUrl = imageUrl;
            request.ModifiedAt = _timeService.GetCurrentTime();
            request.ModifiedBy = _claimsService.GetCurrentUserId;

            // Create a withdrawal transaction
            var transaction = new Transaction
            {
                Amount = request.Amount,
                TransactionDate = _timeService.GetCurrentTime(),
                TransactionStatus = TransactionStatusEnums.COMPLETED.ToString(),
                CreatedAt = _timeService.GetCurrentTime(),
                CreatedBy = _claimsService.GetCurrentUserId
            };
            _context.Transactions.Add(transaction);

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
    }
}