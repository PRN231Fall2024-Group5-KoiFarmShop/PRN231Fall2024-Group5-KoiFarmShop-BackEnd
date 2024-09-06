namespace Koi.Repositories.Helper
{
    public class OrderParams : PaginationParams
    {
        public string? OrderBy { get; set; }

        // [BindProperty(Name = "search-term")]
        public string? SearchTerm { get; set; }

        // [BindProperty(Name = "from-date")]
        public DateTime? FromDate { get; set; }

        //  [BindProperty(Name = "to-date")]
        public DateTime? ToDate { get; set; }
    }
}