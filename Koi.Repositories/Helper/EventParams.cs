namespace Koi.Repositories.Helper
{
    public class EventParams : PaginationParams
    {
        //public string? OrderBy { get; set; }
        //  [BindProperty(Name = "search-term")]
        public string? SearchTerm { get; set; }

        //  [BindProperty(Name = "event-category-id")]
        public int? EventCategoryId { get; set; }

        public int? UserId { get; set; }

        //  [BindProperty(Name = "donation-start-date")]
        public DateTime? EventStartDate { get; set; }

        //  [BindProperty(Name = "event-end-date")]
        public DateTime? EventEndDate { get; set; }
    }
}