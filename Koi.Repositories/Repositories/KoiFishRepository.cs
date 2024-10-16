using Koi.BusinessObjects;
using Koi.Repositories.Helper;
using Koi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Koi.Repositories.Repositories
{
    public class KoiFishRepository : GenericRepository<KoiFish>, IKoiFishRepository
    {
        private readonly KoiFarmShopDbContext _dbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public KoiFishRepository(KoiFarmShopDbContext context, ICurrentTime timeService, IClaimsService claims) : base(context, timeService, claims)

        {
            _dbContext = context;
            _timeService = timeService;
            _claimsService = claims;
        }

        public IQueryable<KoiFish> FilterAllField(KoiParams koiParams)
        {
            var query = _dbContext.KoiFishs
            .Include(x => x.KoiBreeds.Where(y => y.IsDeleted == false))
            .Include(x => x.KoiFishImages.Where(y => y.IsDeleted == false))
            .Include(x => x.KoiDiaries.Where(y => y.IsDeleted == false))
            .Include(x => x.KoiCertificates.Where(y => y.IsDeleted == false))
            .Where(x => x.IsDeleted == false);
            return query;
        }

        public IQueryable<KoiFish> FilterAllField()
        {
            var query = _dbContext.KoiFishs
            .Include(x => x.KoiBreeds.Where(y => y.IsDeleted == false))
            .Include(x => x.KoiFishImages.Where(y => y.IsDeleted == false))
            .Include(x => x.KoiDiaries.Where(y => y.IsDeleted == false))
            .Where(x => x.IsDeleted == false);
            return query;
        }
    }
}