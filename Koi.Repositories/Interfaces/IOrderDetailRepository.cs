using Koi.BusinessObjects;

namespace Koi.Repositories.Interfaces
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {
        public Task<OrderDetail> ChangeToConsigned(int id);

        public Task<OrderDetail> ChangeToCompleted(int id);

        public Task<OrderDetail> ChangeToCanceled(int id);

        public Task<OrderDetail> ChangeToShipping(int id);

        Task<List<OrderDetail>> GetAssignedOrderDetails(int staffId);
    }
}