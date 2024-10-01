using Koi.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Interfaces
{
    public interface IKoiCertificateRepository : IGenericRepository<KoiCertificate>
    {
        Task<List<KoiCertificate>> GetListCertificateByKoiIdAsync(int koiId);
    }
}
