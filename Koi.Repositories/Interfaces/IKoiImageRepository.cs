using Koi.BusinessObjects;

namespace Koi.Repositories.Interfaces
{
    public interface IKoiImageRepository : IGenericRepository<KoiFishImage>
    {
        public Task<KoiFishImage> GetByUrl(string url);
    }
}
