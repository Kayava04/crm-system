namespace CRMSystem.WebAPI.Entities.School
{
    public class StudentGroupEntity
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid GroupId { get; set; }
        public string Level { get; set; } = string.Empty;
        public int PairNumber { get; set; }
        
        public StudentEntity Student { get; set; } = null!;
        public GroupEntity Group { get; set; } = null!;
    }
}