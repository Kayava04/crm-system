using CRMSystem.WebAPI.Entities.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.WebAPI.Configurations.School
{
    public class GroupLessonDayConfiguration : IEntityTypeConfiguration<GroupLessonDayEntity>
    {
        public void Configure(EntityTypeBuilder<GroupLessonDayEntity> builder)
        {
            builder.HasKey(gld => new { gld.GroupId, gld.LessonDayId });

            builder.HasOne(gld => gld.Group)
                .WithMany(g => g.GroupLessonDays)
                .HasForeignKey(gld => gld.GroupId);

            builder.HasOne(gld => gld.LessonDay)
                .WithMany(ld => ld.GroupLessonDays)
                .HasForeignKey(gld => gld.LessonDayId);
        }
    }
}