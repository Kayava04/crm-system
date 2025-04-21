namespace CRMSystem.WebAPI.Models
{
    public class GroupLessonDay
    {
        public Guid GroupId { get; private set; }
        public Guid LessonDayId { get; private set; }

        private GroupLessonDay(Guid groupId, Guid lessonDayId)
        {
            GroupId = groupId;
            LessonDayId = lessonDayId;
        }

        public static GroupLessonDay Create(Guid groupId, Guid lessonDayId) =>
            new(groupId, lessonDayId);
    }
}