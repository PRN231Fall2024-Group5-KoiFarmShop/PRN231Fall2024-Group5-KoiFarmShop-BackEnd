using Koi.Repositories.Entities;
using Koi.Repositories.Helper;

namespace Koi.Repositories.Interfaces
{
    public interface IKoiFishRepository : IGenericRepository<KoiFish>
    {
        public IQueryable<KoiFish> FilterAllField(KoiParams koiParams);
    }
}
