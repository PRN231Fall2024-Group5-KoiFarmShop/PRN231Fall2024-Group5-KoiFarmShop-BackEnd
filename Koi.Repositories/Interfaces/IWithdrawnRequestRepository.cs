using Koi.BusinessObjects;
using Koi.DTOs.WalletDTOs;

namespace Koi.Repositories.Interfaces
{
    public interface IWithdrawnRequestRepository : IGenericRepository<WithdrawnRequest>
    {
        Task<WithdrawnRequest> CreateARequest(WithdrawnRequest request);
        Task<List<WithdrawnRequest>> GetRequestByUserId();
        Task<List<WithdrawnRequest>> GetAllRequest();
        Task<WithdrawnRequest> ApproveRequest(int requestId, string imageUrl);
        Task<WithdrawnRequest> RejectRequest(int requestId);
        Task<DashboardOrderStatisticsDto> Analyst(DateTime startDate, DateTime endDate);
    }
}
