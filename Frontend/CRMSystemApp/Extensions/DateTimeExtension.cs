namespace CRMSystemApp.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ToUtcDate(this DateTime date) =>
            DateTime.SpecifyKind(date, DateTimeKind.Utc);

        public static DateTime ToUtcDate(this DateTime? date) =>
            DateTime.SpecifyKind(date ?? DateTime.UtcNow, DateTimeKind.Utc);
    }
}