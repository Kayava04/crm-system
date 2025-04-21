namespace CRMSystem.WebAPI.Entities.School
{
    public class LanguageEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public ICollection<GroupEntity> Groups { get; set; } = new List<GroupEntity>();
    }
}