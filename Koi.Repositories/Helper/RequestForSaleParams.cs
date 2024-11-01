using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Helper
{
  public class RequestForSaleParams : PaginationParams
  {
    public int? UserId { get; set; }
    public int? KoiFishId { get; set; }
    public String? RequestStatus { get; set; }
  }
}
