﻿using Koi.BusinessObjects;

namespace Koi.Repositories.Interfaces
{
    public interface IConsignmentForNurtureRepository : IGenericRepository<ConsignmentForNurture>
    {
        Task<ConsignmentForNurture> AddNurtureConsignmentAsync(ConsignmentForNurture body);

        Task<List<ConsignmentForNurture>> GetAssignedConsignments(int staffId);
    }
}