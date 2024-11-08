using Koi.BusinessObjects;
using Koi.Repositories.Interfaces;

namespace Koi.Repositories.Repositories
{
    public class FAQRepository : GenericRepository<FAQ>, IFAQRepository
    {
        private readonly KoiFarmShopDbContext _dbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public FAQRepository(KoiFarmShopDbContext context, ICurrentTime timeService, IClaimsService claims) : base(context, timeService, claims)

        {
            _dbContext = context;
            _timeService = timeService;
            _claimsService = claims;
        }
    }
}
