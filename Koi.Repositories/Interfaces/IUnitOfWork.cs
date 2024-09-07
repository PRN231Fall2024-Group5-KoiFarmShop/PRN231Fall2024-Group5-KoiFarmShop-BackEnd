﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangeAsync();
    }
}