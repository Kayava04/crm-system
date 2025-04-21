namespace CRMSystem.WebAPI.Models
{
    public class Employee
    {
        public Guid Id { get; private set; }
        public Guid PersonId { get; private set; }
        public string Position { get; private set; }
        public float Salary { get; private set; }

        private Employee(Guid id, Guid personId, string position, float salary)
        {
            Id = id;
            PersonId = personId;
            Position = position;
            Salary = salary;
        }

        public static Employee Create(Guid id, Guid personId, string position, float salary) =>
            new(id, personId, position, salary);
        
        public void Update(string position, float salary) =>
            (Position, Salary) = (position, salary);
    }
}