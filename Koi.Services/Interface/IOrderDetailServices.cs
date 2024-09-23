using Koi.BusinessObjects;

namespace Koi.Services.Interface
{
    public interface IOrderDetailServices
    {
        public Task<Order> ChangeToConsigned(int id);
        public Task<Order> ChangeToCompleted(int id);
        //public Task<Order> ChangeToCanceled(int id);
        public Task<Order> ChangeToShipping(int id);
    }
}
