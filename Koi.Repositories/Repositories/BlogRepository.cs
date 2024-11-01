using Koi.BusinessObjects;
using Koi.Repositories.Commons;
using Koi.Repositories.Helper;
using Koi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Repositories
{
  public class BlogRepository : GenericRepository<Blog>, IBlogRepository
  {
    private readonly KoiFarmShopDbContext _dbContext;
    private readonly ICurrentTime _timeService;
    private readonly IClaimsService _claimsService;

    public BlogRepository(KoiFarmShopDbContext context, ICurrentTime timeService, IClaimsService claims) : base(context, timeService, claims)

    {
      _dbContext = context;
      _timeService = timeService;
      _claimsService = claims;
    }

  }
}
