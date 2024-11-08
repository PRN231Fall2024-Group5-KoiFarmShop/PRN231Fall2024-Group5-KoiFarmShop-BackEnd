using Koi.BusinessObjects;
using Koi.DTOs.WalletDTOs;

namespace Koi.Services.Interface
{
    public interface IWithdrawnRequestService
    {
        Task<WithdrawnRequest> CreateARequest(WithdrawnRequest request);
        Task<List<WithdrawnRequest>> GetRequestByUserId();
        Task<List<WithdrawnRequest>> GetAllRequest();
        Task<WithdrawnRequest> ApproveRequest(int requestId, string imageUrl);
        Task<WithdrawnRequest> RejectRequest(int requestId);
        Task<DashboardOrderStatisticsDto> Analyst(DateTime startDate, DateTime endDate);
    }
}
