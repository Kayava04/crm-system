namespace CRMSystem.WebAPI.Entities.School
{
    public class ContractEntity
    {
        public Guid Id { get; set; }
        public string ContractNumber { get; set; } = string.Empty;
        public DateTime SignDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public Guid StudentId { get; set; }
        
        public StudentEntity Student { get; set; } = null!;
    }
}