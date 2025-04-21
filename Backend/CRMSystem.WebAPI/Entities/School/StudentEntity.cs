namespace CRMSystem.WebAPI.Entities.School
{
    public class StudentEntity
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public Guid? ParentId { get; set; }
        public int? Grade { get; set; }
        public bool IsPaid { get; set; }

        public PersonEntity Person { get; set; } = null!;
        public PersonEntity? Parent { get; set; }

        public ICollection<StudentGroupEntity> StudentGroups { get; set; } = new List<StudentGroupEntity>();
        public ICollection<ContractEntity> Contracts { get; set; } = new List<ContractEntity>();
    }
}