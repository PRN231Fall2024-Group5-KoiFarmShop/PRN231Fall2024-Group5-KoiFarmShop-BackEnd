namespace Koi.Repositories.Helper
{
    public class PaginationParams
    {
        private const int MaxPageSize = 50;

        //[BindProperty(Name = "page-number")]
        public int PageNumber { get; set; } = 1;

        public int _pageSize = 6;

        //[BindProperty(Name = "page-size")]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}