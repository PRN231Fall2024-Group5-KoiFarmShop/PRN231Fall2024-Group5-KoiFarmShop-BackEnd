namespace Koi.Repositories.Models.TestDTO
{
    public class CustomerTestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<OrderTestDTO> Orders { get; set; }
    }
}
