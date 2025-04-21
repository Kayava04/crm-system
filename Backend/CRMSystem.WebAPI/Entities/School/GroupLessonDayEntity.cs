namespace CRMSystem.WebAPI.Entities.School
{
    public class GroupLessonDayEntity
    {
        public Guid GroupId { get; set; }
        public Guid LessonDayId { get; set; }
    
        public GroupEntity Group { get; set; } = null!;
        public LessonDayEntity LessonDay { get; set; } = null!;
    }
}