using Koi.Repositories.Entities;
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
            .Include(x => x.KoiFishKoiBreeds)
            .Include(x => x.Consigner);
            return query;
        }
    }
}
