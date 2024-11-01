using AutoMapper;
using Koi.DTOs.BlogDTOs;
using Koi.Repositories.Commons;
using Koi.Repositories.Helper;
using Koi.Services.Interface;
using Koi.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Koi.WebAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BlogController : ControllerBase
  {
    private readonly IBlogService _blogService;
    private readonly IMapper _mapper;
    public BlogController(IBlogService blogService, IMapper mapper)
    {
      _blogService = blogService;
      _mapper = mapper;
    }
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromQuery] BlogParams blogParams)
    {
      try
      {
        var blogs = await _blogService.GetBlogs(blogParams);

        //Response.AddPaginationHeader(breeds.MetaData);

        var blogReponseDTOs = _mapper.Map<List<BlogResponseDTO>>(blogs);

        return Ok(ApiResult<List<BlogResponseDTO>>.Succeed(blogReponseDTOs, "Get list blogs successfully"));
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
      }
    }
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(int id)
    {
      try
      {
        var blogModel = await _blogService.GetBlogById(id);
        return Ok(ApiResult<BlogResponseDTO>.Succeed(blogModel, "Get Blog Successfully!"));
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains("400"))
          return BadRequest(ApiResult<object>.Fail(ex));
        if (ex.Message.Contains("404"))
          return NotFound(ApiResult<object>.Fail(ex));

        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
      }
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> Post([FromBody] CreateBlogDTO blog)
    {
      try
      {
        var blogModel = await _blogService.CreateBlog(blog);
        return Created();
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains("400"))
          return BadRequest(ApiResult<object>.Fail(ex));
        if (ex.Message.Contains("404"))
          return NotFound(ApiResult<object>.Fail(ex));
        if (ex.Message.Contains("501"))
          return StatusCode(StatusCodes.Status501NotImplemented, ex.Message);
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateBlogDTO blog)
    {
      try
      {
        var result = await _blogService.UpdateBlog(blog, id);
        return Ok(ApiResult<BlogResponseDTO>.Succeed(result, "Update Blog Successfully!"));
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains("400"))
          return BadRequest(ApiResult<object>.Fail(ex));
        if (ex.Message.Contains("404"))
          return NotFound(ApiResult<object>.Fail(ex));
        if (ex.Message.Contains("501"))
          return StatusCode(StatusCodes.Status501NotImplemented, ex.Message);
        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
      }
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        var result = await _blogService.DeleteBlog(id);
        return Ok(ApiResult<object>.Succeed(null, "Delete Blog Successfully!"));
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains("400"))
          return BadRequest(ApiResult<object>.Fail(ex));
        if (ex.Message.Contains("404"))
          return NotFound(ApiResult<object>.Fail(ex));
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }
  }
}
