namespace CRMSystem.WebAPI.Core
{
    public class DbInitializer
    {
        public static void Initialize(SchoolDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}