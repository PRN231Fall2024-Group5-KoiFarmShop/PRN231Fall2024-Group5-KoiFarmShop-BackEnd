using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Helper
{
    public class KoiCertificateParams : PaginationParams
    {
        public string? KoiName {  get; set; }
        public string? UserName { get; set; }
    }
}
