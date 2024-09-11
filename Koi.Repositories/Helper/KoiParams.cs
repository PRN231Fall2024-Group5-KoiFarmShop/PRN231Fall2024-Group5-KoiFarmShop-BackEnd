namespace Koi.Repositories.Helper
{
    public class KoiParams : PaginationParams
    {
        //public string? OrderBy { get; set; }
        //  [BindProperty(Name = "search-term")]
        public string? SearchTerm { get; set; }

        //  [BindProperty(Name = "koi-breed-id")]
        public int? KoiBreedId { get; set; }

        //  [BindProperty(Name = "user-id")]
        public int? UserId { get; set; }

        //  [BindProperty(Name = "age")]
        public int? Age { get; set; }

        //  [BindProperty(Name = "gender")]
        public bool? Gender { get; set; }

        //  [BindProperty(Name = "is-available-for-sale")]
        public bool? IsAvailableForSale { get; set; }

        //  [BindProperty(Name = "is-consigned")]
        public bool? IsConsigned { get; set; }

        //  [BindProperty(Name = "length")]
        public int? Length { get; set; }

        //  [BindProperty(Name = "weight")]
        public int? Weight { get; set; }

        //  [BindProperty(Name = "origin")]
        public string? Origin { get; set; }

        //  [BindProperty(Name = "lower-price")]
        public Int64? LowerPrice { get; set; }

        //  [BindProperty(Name = "upper-price")]
        public Int64? UpperPrice { get; set; }

    }
}