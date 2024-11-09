using Koi.BusinessObjects;
using Koi.DTOs.RequestForSaleDTOs;
using Koi.Repositories.Helper;

namespace Koi.Services.Interface
{
    public interface IRequestForSaleService
    {
        IQueryable<RequestForSale> GetRequestForSales();
        IQueryable<RequestForSale> GetMyRequestForSales();
        Task<RequestForSaleResponseDTO> UpdateRequestForSale(RequestForSaleUpdateDTO dto, int id);
        Task<RequestForSaleResponseDTO> CreateRequestForSale(RequestForSaleCreateDTO dto);
        Task<List<RequestForSaleResponseDTO>> GetRequestForSales(RequestForSaleParams requestForSaleParams);
        Task<RequestForSaleResponseDTO> GetRequestForSaleById(int id);
        Task<RequestForSaleResponseDTO> ApproveRequest(int id);
        Task<RequestForSaleResponseDTO> RejectRequest(int id, string reason);
        Task<RequestForSaleResponseDTO> CancelRequest(int id);
        Task<bool> DeleteRequestForSale(int id);

    }
}
