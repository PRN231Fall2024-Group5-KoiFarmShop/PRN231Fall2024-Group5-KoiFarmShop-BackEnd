namespace Koi.BusinessObjects
{
    public class Diet : BaseEntity
    {
        public string Name { get; set; }
        public int DietCost { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ConsignmentForNurture> Consignments { get; set; }
    }
}