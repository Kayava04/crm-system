using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Entities.Auth;
using CRMSystem.WebAPI.Entities.Email;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.WebAPI.Core
{
    public class SchoolDbContext(DbContextOptions<SchoolDbContext> options)
        : DbContext(options)
    {
        // School
        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<ContractEntity> Contracts { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<GroupLessonDayEntity> GroupLessonDays { get; set; }
        public DbSet<LanguageEntity> Languages { get; set; }
        public DbSet<LessonDayEntity> LessonDays { get; set; }
        public DbSet<PersonEntity> Persons { get; set; }
        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<StudentGroupEntity> StudentGroups { get; set; }
        
        // Auth
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        
        // Email
        public DbSet<MessageEntity> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SchoolDbContext).Assembly);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}