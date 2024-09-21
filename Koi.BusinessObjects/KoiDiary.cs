namespace Koi.BusinessObjects
{
    public class KoiDiary : BaseEntity
    {
        public int KoiFishId { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public virtual KoiFish KoiFish { get; set; }
    }
}