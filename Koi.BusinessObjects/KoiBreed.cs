namespace Koi.BusinessObjects
{
    public class KoiBreed : BaseEntity
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }

        // Navigation property
        //public virtual ICollection<KoiFishKoiBreed> KoiFishKoiBreeds { get; set; }
        public virtual ICollection<KoiFish> KoiFishes { get; set; }
    }
}