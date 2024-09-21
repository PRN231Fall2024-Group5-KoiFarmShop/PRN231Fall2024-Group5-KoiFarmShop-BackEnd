namespace Koi.DTOs.KoiFishDTOs
{
    public class KoiFishDiaryCreateDTO
    {
        public int KoiFishId { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}
