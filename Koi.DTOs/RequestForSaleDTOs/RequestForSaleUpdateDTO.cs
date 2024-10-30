using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.RequestForSaleDTOs
{
  public class RequestForSaleUpdateDTO
  {
    public int Id { get; set; }
    // public int UserId { get; set; }
    // public int KoiFishId { get; set; }
    public Int64? PriceDealed { get; set; }
    public String? Note { get; set; }

  }
}
