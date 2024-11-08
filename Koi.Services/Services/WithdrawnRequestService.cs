using Koi.BusinessObjects;
using Koi.DTOs.WalletDTOs;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;

namespace Koi.Services.Services
{
    public class WithdrawnRequestService : IWithdrawnRequestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WithdrawnRequestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<WithdrawnRequest> CreateARequest(WithdrawnRequest request)
        {
            return await _unitOfWork.WithdrawnRequestRepository.CreateARequest(request);
        }

        public async Task<List<WithdrawnRequest>> GetRequestByUserId()
        {
            return await _unitOfWork.WithdrawnRequestRepository.GetRequestByUserId();
        }

        public async Task<List<WithdrawnRequest>> GetAllRequest()
        {
            return await _unitOfWork.WithdrawnRequestRepository.GetAllRequest();
        }

        public async Task<WithdrawnRequest> ApproveRequest(int requestId, string imageUrl)
        {
            return await _unitOfWork.WithdrawnRequestRepository.ApproveRequest(requestId, imageUrl);
        }

        public async Task<WithdrawnRequest> RejectRequest(int requestId)
        {
            return await _unitOfWork.WithdrawnRequestRepository.RejectRequest(requestId);
        }

        public Task<DashboardOrderStatisticsDto> Analyst(DateTime startDate, DateTime endDate)
        {
            return _unitOfWork.WithdrawnRequestRepository.Analyst(startDate, endDate);
        }
    }
}
