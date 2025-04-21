namespace CRMSystem.WebAPI.Models
{
    public class Contact
    {
        public Guid Id { get; private set; }
        public Guid PersonId { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }

        private Contact(Guid id, Guid personId, string phone, string email)
        {
            Id = id;
            PersonId = personId;
            Phone = phone;
            Email = email;
        }

        public static Contact Create(Guid id, Guid personId, string phone, string email) =>
            new(id, personId, phone, email);
        
        public void Update(string phone, string email) =>
            (Phone, Email) = (phone, email);
    }
}