namespace CRMSystem.WebAPI.Entities.School
{
    public class EmployeeEntity
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public string Position { get; set; } = string.Empty;
        public float Salary { get; set; }
        
        public PersonEntity Person { get; set; } = null!;
    }
}