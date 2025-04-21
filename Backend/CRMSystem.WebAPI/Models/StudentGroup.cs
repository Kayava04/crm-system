namespace CRMSystem.WebAPI.Models
{
    public class StudentGroup
    {
        public Guid Id { get; private set; }
        public Guid StudentId { get; private set; }
        public Guid GroupId { get; private set; }
        public string Level { get; private set; }
        public int PairNumber { get; private set; }

        private StudentGroup(Guid id, Guid studentId, Guid groupId, string level, int pairNumber)
        {
            Id = id;
            StudentId = studentId;
            GroupId = groupId;
            Level = level;
            PairNumber = pairNumber;
        }

        public static StudentGroup Create(Guid id, Guid studentId, Guid groupId, string level, int pairNumber) =>
            new(id, studentId, groupId, level, pairNumber);
        
        public void Update(Guid studentId, Guid groupId, string level, int pairNumber) =>
            (StudentId, GroupId, Level, PairNumber) = (studentId, groupId, level, pairNumber);
    }
}