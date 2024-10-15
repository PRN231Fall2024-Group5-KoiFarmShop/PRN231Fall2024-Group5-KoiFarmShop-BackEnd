using Koi.BusinessObjects;
using Koi.Repositories.Helper;

namespace Koi.Repositories.Interfaces
{
    public interface IKoiFishRepository : IGenericRepository<KoiFish>
    {
        public IQueryable<KoiFish> FilterAllField(KoiParams koiParams);
        public IQueryable<KoiFish> FilterAllField();
    }
}
