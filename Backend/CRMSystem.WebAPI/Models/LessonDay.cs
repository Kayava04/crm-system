namespace CRMSystem.WebAPI.Models
{
    public class LessonDay
    {
        public Guid Id { get; private set; }
        public string DayOfWeek { get; private set; }

        private LessonDay(Guid id, string dayOfWeek)
        {
            Id = id;
            DayOfWeek = dayOfWeek;
        }

        public static LessonDay Create(Guid id, string dayOfWeek) =>
            new(id, dayOfWeek);
    }
}