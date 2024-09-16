namespace Koi.BusinessObjects
{
    public class KoiFishKoiBreed
    {
        public int KoiFishId { get; set; } // Foreign key
        //public virtual KoiFish KoiFish { get; set; } // Navigation property

        public int KoiBreedId { get; set; } // Foreign key
        //public virtual KoiBreed KoiBreed { get; set; } // Navigation property
    }
}