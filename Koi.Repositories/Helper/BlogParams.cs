namespace Koi.Repositories.Helper
{
    public class BlogParams : PaginationParams
    {
        public string? Title { get; set; }
        public bool? IsNews { get; set; }
        public bool? IsPublished { get; set; }
    }
}
