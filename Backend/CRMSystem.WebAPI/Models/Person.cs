namespace CRMSystem.WebAPI.Models
{
    public class Person
    {
        public Guid Id { get; private set; }
        public string FullName { get; private set; }
        public DateTime BirthDate { get; private set; }

        private Person(Guid id, string fullName, DateTime birthDate)
        {
            Id = id;
            FullName = fullName;
            BirthDate = birthDate;
        }

        public static Person Create(Guid id, string fullName, DateTime birthDate) =>
            new(id, fullName, birthDate);
        
        public void Update(string fullName, DateTime birthDate) =>
            (FullName, BirthDate) = (fullName, birthDate);
    }
}