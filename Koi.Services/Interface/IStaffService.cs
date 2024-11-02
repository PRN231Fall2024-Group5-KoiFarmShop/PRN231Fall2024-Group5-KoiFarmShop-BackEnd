using Koi.BusinessObjects;
using Koi.DTOs.ConsignmentDTOs;
using Koi.DTOs.Enums;
using Koi.DTOs.PaymentDTOs;
using Koi.Repositories.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Services.Interface
{
    public interface IStaffService
    {
        Task<ApiResult<OrderDetailDTO>> AssignStaffOrderDetail(int id, int staffId);

        Task<OrderDetailDTO> ChangeToCompleted(int id);

        Task<OrderDetailDTO> ChangeToConsigned(int id);

        Task<OrderDetailDTO> ChangeToGettingFish(int id);

        Task<OrderDetailDTO> ChangeToShipping(int id);

        Task<List<ConsignmentForNurtureDetailDTO>> GetAssignedConsigntment(int staffId);

        Task<List<OrderDetailDTO>> OrderDetailDTO(int staffId);

        Task<ConsignmentForNurtureDTO> UpdateConsignmentStatusOnlyAsync(int consignmentId, ConsignmentStatusEnums newStatus);
    }
}