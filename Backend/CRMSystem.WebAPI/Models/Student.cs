namespace CRMSystem.WebAPI.Models
{
    public class Student
    {
        public Guid Id { get; private set; }
        public Guid PersonId { get; private set; }
        public Guid? ParentId { get; private set; }
        public int? Grade { get; private set; }
        public bool IsPaid { get; private set; }

        private Student(Guid id, Guid personId, Guid? parentId, int? grade, bool isPaid)
        {
            Id = id;
            PersonId = personId;
            ParentId = parentId;
            Grade = grade;
            IsPaid = isPaid;
        }

        public static Student Create(Guid id, Guid personId, Guid? parentId, int? grade, bool isPaid) =>
            new(id, personId, parentId, grade, isPaid);
        
        public void Update(Guid personId, Guid? parentId, int? grade, bool isPaid) =>
            (PersonId, ParentId, Grade, IsPaid) = (personId, parentId, grade, isPaid);
    }
}