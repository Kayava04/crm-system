namespace CRMSystem.WebAPI.Models
{
    public class Language
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        private Language(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Language Create(Guid id, string name) =>
            new(id, name);
    }
}