﻿using Koi.Repositories.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories
{
    public class KoiFarmShopDbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<KoiFish> KoiFishs { get; set; }

        public KoiFarmShopDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}