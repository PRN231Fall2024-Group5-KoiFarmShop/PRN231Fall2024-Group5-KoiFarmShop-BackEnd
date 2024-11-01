using Koi.BusinessObjects;
using Koi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Repositories
{
    public class ConsignmentForNurtureRepository : GenericRepository<ConsignmentForNurture>, IConsignmentForNurtureRepository
    {
        private readonly KoiFarmShopDbContext _dbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public ConsignmentForNurtureRepository(KoiFarmShopDbContext context, ICurrentTime timeService, IClaimsService claims) : base(context, timeService, claims)
        {
            _dbContext = context;
            _timeService = timeService;
            _claimsService = claims;
        }

        public async Task<ConsignmentForNurture> AddNurtureConsignmentAsync(ConsignmentForNurture body)
        {
            body.ConsignmentDate = _timeService.GetCurrentTime();
            var result = await AddAsync(body);
            return result;
        }

        public async Task<List<ConsignmentForNurture>> GetAssignedConsignments(int staffId)
        {
            var result = await _dbContext.ConsignmentForNurtures.Include(x => x.KoiFish).Include(x => x.Diet).Where(x => x.StaffId == staffId).ToListAsync();
            return result;
        }
    }
}