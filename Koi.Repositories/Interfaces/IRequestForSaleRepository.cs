
using Koi.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Interfaces
{
  public interface IRequestForSaleRepository : IGenericRepository<RequestForSale>
  {
    IQueryable<RequestForSale> GetRequestForSales();
  }
}