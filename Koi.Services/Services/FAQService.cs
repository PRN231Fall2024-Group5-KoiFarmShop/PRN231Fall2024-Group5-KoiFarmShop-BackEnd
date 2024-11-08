using Koi.BusinessObjects;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;

namespace Koi.Services.Services
{
    public class FAQService : IFAQService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FAQService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<FAQ> CreateFAQ(FAQ fAQ)
        {
            await _unitOfWork.FAQRepository.AddAsync(fAQ);
            await _unitOfWork.SaveChangeAsync();
            return fAQ;
        }

        public async Task<bool> DeleteFAQ(int id)
        {
            var fAQ = await _unitOfWork.FAQRepository.GetByIdAsync(id);
            if (fAQ == null)
            {
                return false;
            }
            await _unitOfWork.FAQRepository.SoftRemove(fAQ);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        public async Task<FAQ> GetFAQById(int id)
        {
            return await _unitOfWork.FAQRepository.GetByIdAsync(id);
        }

        public async Task<List<FAQ>> GetFAQs()
        {
            return await _unitOfWork.FAQRepository.GetAllAsync();
        }

        public async Task<FAQ> UpdateFAQ(int id, FAQ fAQ)
        {
            //get the FAQ
            var fAQFromDb = await _unitOfWork.FAQRepository.GetByIdAsync(id);
            if (fAQFromDb == null)
            {
                return null;
            }
            //update the FAQ
            fAQFromDb.Question = fAQ.Question;
            fAQFromDb.Answer = fAQ.Answer;
            var isSuccess = await _unitOfWork.FAQRepository.Update(fAQFromDb);
            await _unitOfWork.SaveChangeAsync();
            return isSuccess ? fAQFromDb : null;
        }
    }
}
