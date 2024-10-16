using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.BlogDTOs;
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
  public class BlogService : IBlogService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public BlogService(IUnitOfWork unitOfWork, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }
    public async Task<BlogResponseDTO> GetBlogById(int id)
    {
      try
      {
        var certificate = await _unitOfWork.KoiCertificateRepository.GetByIdAsync(id);
        if (certificate == null)
        {
          throw new Exception("404 - Certificate not found!");
        }
        var result = _mapper.Map<BlogResponseDTO>(certificate);
        return result;
      }
      catch (Exception ex)
      {
        throw ex;
      }

    }
    public async Task<List<BlogResponseDTO>> GetBlogs(BlogParams blogParams)
    {
      try
      {
        var list = await _unitOfWork.BlogRepository.GetAllAsync();

        if (!string.IsNullOrEmpty(blogParams.Title))
        {
          list = list
              .Where(x => x.Title.ToLower().Contains(blogParams.Title.ToLower()))
              .ToList();
        }
        if (!string.IsNullOrEmpty(blogParams.AuthorEmail))
        {
          list = list
                .Where(x => x.Author.Email.ToLower().Contains(blogParams.AuthorEmail.ToLower()))
              .ToList();
        }
        if (blogParams.IsNews != null)
        {
          list = list
                .Where(x => x.IsNews == blogParams.IsNews)
                .ToList();
        }
        if (blogParams.IsPublished != null)
        {
          list = list
                .Where(x => x.IsPublished == blogParams.IsPublished)
                .ToList();
        }
        var result = _mapper.Map<List<BlogResponseDTO>>(list);
        result = result.Skip((blogParams.PageNumber - 1) * blogParams.PageSize)
                        .Take(blogParams.PageSize)
                        .ToList();
        return result;

      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public async Task<BlogResponseDTO> CreateBlog(CreateBlogDTO dto)
    {
      try
      {
        var existingBlogs = await _unitOfWork.BlogRepository.GetAllAsync();
        var isExist = existingBlogs.FirstOrDefault(x => x.Title.ToLower() == dto.Title.ToLower());
        if (isExist != null)
        {
          throw new Exception("400 - Create failed. Blog has already existed!");
        }
        var author = await _unitOfWork.UserRepository.GetUserByEmailAsync(dto.AuthorEmail);
        if (author == null)
        {
          throw new Exception("400 - Create failed. Author not found!");
        }
        var _blog = new Blog()
        {
          Title = dto.Title,
          Author = author,
          Content = dto.Content,
        };
        var newBlog = await _unitOfWork.BlogRepository.AddAsync(_blog);
        var result = _mapper.Map<BlogResponseDTO>(newBlog);
        int check = await _unitOfWork.SaveChangeAsync();
        if (check < 0)
        {
          throw new Exception("400 - Create failed");
        }
        return result;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public async Task<BlogResponseDTO> UpdateBlog(UpdateBlogDTO dto, int id)
    {
      try
      {
        var existingBlog = await _unitOfWork.BlogRepository.GetByIdAsync(id);
        if (existingBlog == null)
        {
          throw new Exception("400 - Update failed");
        }
        if (!string.IsNullOrEmpty(dto.Title))
        {
          existingBlog.Title = dto.Title;
        }
        if (!string.IsNullOrEmpty(dto.AuthorEmail))
        {
          var author = await _unitOfWork.UserRepository.GetUserByEmailAsync(dto.AuthorEmail);
          if (author == null)
          {
            throw new Exception("400 - Update failed. Author not found!");
          }
          existingBlog.Author = author;
        }
        if (!string.IsNullOrEmpty(dto.Content))
        {
          existingBlog.Content = dto.Content;
        }
        var check = await _unitOfWork.BlogRepository.Update(existingBlog);
        if (check == false)
        {
          throw new Exception("400 - Update failed");
        }
        var result = _mapper.Map<BlogResponseDTO>(existingBlog);
        await _unitOfWork.SaveChangeAsync();
        return result;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public async Task<bool> DeleteBlog(int id)
    {
      try
      {
        var existingBlog = await _unitOfWork.BlogRepository.GetByIdAsync(id);
        if (existingBlog == null)
        {
          throw new Exception("400 - Delete failed");
        }
        var check = await _unitOfWork.BlogRepository.SoftRemove(existingBlog);
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
  }
}
