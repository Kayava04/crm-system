using CRMSystem.WebAPI.Entities.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.WebAPI.Configurations.School
{
    public class LessonDayConfiguration : IEntityTypeConfiguration<LessonDayEntity>
    {
        public void Configure(EntityTypeBuilder<LessonDayEntity> builder)
        {
            builder.HasKey(ld => ld.Id);

            builder.Property(ld => ld.DayOfWeek)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}