using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Helper
{
  public class BlogParams : PaginationParams
  {
    public string? Title { get; set; }
    public string? AuthorEmail { get; set; }
    public bool? IsNews { get; set; }
    public bool? IsPublished { get; set; }
  }
}
