using Koi.DTOs.BlogDTOs;
using Koi.Repositories.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Services.Interface
{
  public interface IBlogService
  {
    Task<bool> DeleteBlog(int id);
    Task<BlogResponseDTO> UpdateBlog(UpdateBlogDTO dto, int id);
    Task<BlogResponseDTO> CreateBlog(CreateBlogDTO dto);
    Task<List<BlogResponseDTO>> GetBlogs(BlogParams blogParams);
    Task<BlogResponseDTO> GetBlogById(int id);
  }
}
