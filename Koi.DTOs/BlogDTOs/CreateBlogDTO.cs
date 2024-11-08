namespace Koi.DTOs.BlogDTOs
{
    public class CreateBlogDTO
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsPublished { get; set; }
        public bool IsNews { get; set; }
        public string? Tags { get; set; }
    }
}
