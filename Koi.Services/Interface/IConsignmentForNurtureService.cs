using Koi.DTOs.ConsignmentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Services.Interface
{
    public interface IConsignmentForNurtureService
    {
        Task<ConsignmentForNurtureDTO> CreateConsignmentAsync(ConsignmentRequestDTO consignmentRequestDTO);
        Task<List<ConsignmentForNurtureDTO>> GetAllConsignmentsAsync();
        Task<ConsignmentForNurtureDTO> GetConsignmentByIdAsync(int consignmentId);
        Task UpdateConsignmentStatusAsync(int consignmentId, string newStatus);
    }
}