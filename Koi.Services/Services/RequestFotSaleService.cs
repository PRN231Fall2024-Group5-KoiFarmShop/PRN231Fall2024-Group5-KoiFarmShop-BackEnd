using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.BlogDTOs;
using Koi.DTOs.RequestForSaleDTOs;
using Koi.Repositories.Helper;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Services.Services
{
  public class RequestForSaleService : IRequestForSaleService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    private readonly IClaimsService _claimsService;
    public RequestForSaleService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
      _claimsService = claimsService;
    }
    public IQueryable<RequestForSale> GetRequestForSales()
    {
      return _unitOfWork.RequestForSaleRepository.GetRequestForSales().AsQueryable();
    }
    public IQueryable<RequestForSale> GetMyRequestForSales()
    {
      var id = _claimsService.GetCurrentUserId;
      return _unitOfWork.RequestForSaleRepository.GetRequestForSales().Where(x => x.UserId == id).AsQueryable();
    }
    public async Task<RequestForSaleResponseDTO> GetRequestForSaleById(int id)
    {
      try
      {
        var requestForSale = await _unitOfWork.RequestForSaleRepository.GetByIdAsync(id);
        if (requestForSale == null)
        {
          throw new Exception("404 - Certificate not found!");
        }
        var result = _mapper.Map<RequestForSaleResponseDTO>(requestForSale);
        return result;
      }
      catch (Exception ex)
      {
        throw ex;
      }

    }
    public async Task<List<RequestForSaleResponseDTO>> GetRequestForSales(RequestForSaleParams requestForSaleParams)
    {
      try
      {
        var list = await _unitOfWork.RequestForSaleRepository.GetAllAsync();

        if (requestForSaleParams.UserId != null)
        {
          list = list
                .Where(x => x.UserId == requestForSaleParams.UserId)
                .ToList();
        }
        if (requestForSaleParams.KoiFishId != null)
        {
          list = list
                .Where(x => x.KoiFishId == requestForSaleParams.KoiFishId)
              .ToList();
        }
        if (requestForSaleParams.RequestStatus != null)
        {
          list = list
                .Where(x => x.RequestStatus == requestForSaleParams.RequestStatus)
                .ToList();
        }
        var result = _mapper.Map<List<RequestForSaleResponseDTO>>(list);
        result = result.Skip((requestForSaleParams.PageNumber - 1) * requestForSaleParams.PageSize)
                        .Take(requestForSaleParams.PageSize)
                        .ToList();
        return result;

      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public async Task<RequestForSaleResponseDTO> CreateRequestForSale(RequestForSaleCreateDTO dto)
    {
      try
      {
        var koiFish = await _unitOfWork.KoiFishRepository.GetByIdAsync(dto.KoiFishId);
        if (koiFish == null)
        {
          throw new Exception("404 - Create failed. Koi fish not found!");
        }

        var id = _claimsService.GetCurrentUserId;
        var user = await _unitOfWork.UserRepository.GetAccountDetailsAsync(id);
        if (user == null)
        {
          throw new Exception("400 - Create failed. User not found!");
        }

        var existingRequestForSales = await _unitOfWork.RequestForSaleRepository.GetAllAsync();
        var isExist = existingRequestForSales.FirstOrDefault(x => x.UserId == user.Id && x.KoiFishId == dto.KoiFishId && x.RequestStatus == "PENDING");
        if (isExist != null)
        {
          throw new Exception("400 - Create failed. Request for sale has already existed!");
        }

        if (dto.PriceDealed <= 0)
        {
          throw new Exception("400 - Create failed. Price must be greater than 0!");
        }

        var _requestForSale = new RequestForSale()
        {
          UserId = user.Id,
          KoiFishId = dto.KoiFishId,
          PriceDealed = dto.PriceDealed,
          RequestStatus = "PENDING",
          Note = dto.Note,
        };

        var newRequestForSale = await _unitOfWork.RequestForSaleRepository.AddAsync(_requestForSale);
        if (newRequestForSale == null)
        {
          throw new Exception("400 - Create failed. Could not add request to database!");
        }

        int check = await _unitOfWork.SaveChangeAsync();
        if (check < 0)
        {
          throw new Exception("400 - Create failed. Could not save changes!");
        }

        var result = _mapper.Map<RequestForSaleResponseDTO>(newRequestForSale);
        return result;
      }
      catch (Exception ex)
      {
        throw new Exception($"500 - Create failed: {ex.Message}");
      }
    }
    public async Task<RequestForSaleResponseDTO> UpdateRequestForSale(RequestForSaleUpdateDTO dto, int id)
    {
      try
      {
        var existingRequestForSale = await _unitOfWork.RequestForSaleRepository.GetByIdAsync(id);
        if (existingRequestForSale == null)
        {
          throw new Exception("400 - Update failed");
        }

        if (existingRequestForSale.RequestStatus == "COMPLETED")
        {
          throw new Exception("400 - Update failed. Only pending request for sale can be updated!");
        }

        // if (dto.UserId != null)
        // {
        //   var user = await _unitOfWork.UserRepository.GetCurrentUserAsync();
        //   if (user == null)
        //   {
        //     throw new Exception("400 - Update failed. User not found!");
        //   }
        //   existingRequestForSale.UserId = user.Id;
        // }
        // if (dto.KoiFishId != null)
        // {
        //   var koiFish = await _unitOfWork.KoiFishRepository.GetByIdAsync(dto.KoiFishId);
        //   if (koiFish == null)
        //   {
        //     throw new Exception("400 - Update failed. Koi fish not found!");
        //   }
        //   existingRequestForSale.KoiFishId = dto.KoiFishId;
        // }
        if (dto.PriceDealed != null)
        {
          existingRequestForSale.PriceDealed = dto.PriceDealed;
        }
        if (dto.Note != null)
        {
          existingRequestForSale.Note = dto.Note;
        }
        existingRequestForSale.RequestStatus = "PENDING";
        var check = await _unitOfWork.RequestForSaleRepository.Update(existingRequestForSale);
        if (check == false)
        {
          throw new Exception("400 - Update failed");
        }
        var result = _mapper.Map<RequestForSaleResponseDTO>(existingRequestForSale);
        await _unitOfWork.SaveChangeAsync();
        return result;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public async Task<bool> DeleteRequestForSale(int id)
    {
      try
      {
        var existingRequestForSale = await _unitOfWork.RequestForSaleRepository.GetByIdAsync(id);
        if (existingRequestForSale == null)
        {
          throw new Exception("400 - Delete failed");
        }
        if (existingRequestForSale.RequestStatus != "PENDING")
        {
          throw new Exception("400 - Delete failed. Only pending request for sale can be deleted!");
        }
        var check = await _unitOfWork.RequestForSaleRepository.SoftRemove(existingRequestForSale);
        if (check == false)
        {
          throw new Exception("400 - Delete failed");
        }
        await _unitOfWork.SaveChangeAsync();
        return check;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public async Task<RequestForSaleResponseDTO> ApproveRequest(int id)
    {
      try
      {
        var existingRequest = await _unitOfWork.RequestForSaleRepository.GetByIdAsync(id);
        if (existingRequest == null)
        {
          throw new Exception("404 - Request not found");
        }

        // Get associated fish
        var koiFish = await _unitOfWork.KoiFishRepository.GetByIdAsync(existingRequest.KoiFishId);
        if (koiFish == null)
        {
          throw new Exception("404 - Associated Koi fish not found");
        }

        existingRequest.RequestStatus = "APPROVED";

        // Update fish details
        koiFish.IsAvailableForSale = true;
        koiFish.Price = existingRequest.PriceDealed ?? 100000;

        await _unitOfWork.KoiFishRepository.Update(koiFish);
        await _unitOfWork.RequestForSaleRepository.Update(existingRequest);
        await _unitOfWork.SaveChangeAsync();

        return _mapper.Map<RequestForSaleResponseDTO>(existingRequest);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public async Task<RequestForSaleResponseDTO> RejectRequest(int id, string reason)
    {
      try
      {
        var existingRequest = await _unitOfWork.RequestForSaleRepository.GetByIdAsync(id);
        if (existingRequest == null)
        {
          throw new Exception("404 - Request not found");
        }

        existingRequest.RequestStatus = "REJECTED";

        // Append rejection reason to note
        string rejectionNote = $"REJECTED for reason: \"{reason}\"";
        existingRequest.Note = string.IsNullOrEmpty(existingRequest.Note)
          ? rejectionNote
          : $"{existingRequest.Note}\n{rejectionNote}";

        await _unitOfWork.RequestForSaleRepository.Update(existingRequest);
        await _unitOfWork.SaveChangeAsync();

        return _mapper.Map<RequestForSaleResponseDTO>(existingRequest);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public async Task<RequestForSaleResponseDTO> CancelRequest(int id)
    {
      try
      {
        var existingRequest = await _unitOfWork.RequestForSaleRepository.GetByIdAsync(id);
        if (existingRequest == null)
        {
          throw new Exception("404 - Request not found");
        }

        existingRequest.RequestStatus = "CANCELED";
        await _unitOfWork.RequestForSaleRepository.Update(existingRequest);
        await _unitOfWork.SaveChangeAsync();

        return _mapper.Map<RequestForSaleResponseDTO>(existingRequest);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
