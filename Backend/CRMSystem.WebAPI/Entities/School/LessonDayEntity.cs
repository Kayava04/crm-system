namespace CRMSystem.WebAPI.Entities.School
{
    public class LessonDayEntity
    {
        public Guid Id { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        
        public ICollection<GroupLessonDayEntity> GroupLessonDays { get; set; } = new List<GroupLessonDayEntity>();
    }
}