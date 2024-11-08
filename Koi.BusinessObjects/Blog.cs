namespace Koi.BusinessObjects
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; }
        public bool IsNews { get; set; }
        public string? ImageUrl { get; set; }
        public string Content { get; set; }
        public string? Tags { get; set; }
        public bool IsPublished { get; set; }
    }
}