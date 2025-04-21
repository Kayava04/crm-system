namespace CRMSystem.WebAPI.Entities.School
{
    public class ContactEntity
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public PersonEntity Person { get; set; } = null!;
    }
}