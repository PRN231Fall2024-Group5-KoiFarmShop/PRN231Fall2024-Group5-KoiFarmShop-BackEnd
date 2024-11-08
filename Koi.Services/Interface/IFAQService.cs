using Koi.BusinessObjects;

namespace Koi.Services.Interface
{
    public interface IFAQService
    {
        Task<bool> DeleteFAQ(int id);
        Task<FAQ> UpdateFAQ(int id, FAQ fAQ);
        Task<FAQ> CreateFAQ(FAQ fAQ);
        Task<List<FAQ>> GetFAQs();
        Task<FAQ> GetFAQById(int id);
    }
}
