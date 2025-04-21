namespace CRMSystem.WebAPI.Entities.School
{
    public class GroupEntity
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public Guid LanguageId { get; set; }
        
        public LanguageEntity Language { get; set; } = null!;
        public ICollection<GroupLessonDayEntity> GroupLessonDays { get; set; } = new List<GroupLessonDayEntity>();
        public ICollection<StudentGroupEntity> StudentGroups { get; set; } = new List<StudentGroupEntity>();
    }
}