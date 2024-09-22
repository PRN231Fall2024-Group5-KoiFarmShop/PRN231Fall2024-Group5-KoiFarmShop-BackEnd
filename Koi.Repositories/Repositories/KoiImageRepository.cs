using Koi.BusinessObjects;
using Koi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Koi.Repositories.Repositories
{
    public class KoiImageRepository : GenericRepository<KoiFishImage>, IKoiImageRepository
    {
        private readonly KoiFarmShopDbContext _dbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public KoiImageRepository(KoiFarmShopDbContext context, ICurrentTime timeService, IClaimsService claims) : base(context, timeService, claims)

        {
            _dbContext = context;
            _timeService = timeService;
            _claimsService = claims;
        }
        public async Task<KoiFishImage> GetByUrl(string url)
        {
            return await _dbContext.KoiFishImages.FirstOrDefaultAsync(x => x.ImageUrl == url);
        }
    }
}
