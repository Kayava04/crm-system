namespace CRMSystem.WebAPI.Entities.School
{
    public class PersonEntity
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        
        public ICollection<ContactEntity> Contacts { get; set; } = new List<ContactEntity>();
    }
}