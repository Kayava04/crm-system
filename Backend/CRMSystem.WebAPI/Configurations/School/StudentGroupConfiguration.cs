using CRMSystem.WebAPI.Entities.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.WebAPI.Configurations.School
{
    public class StudentGroupConfiguration : IEntityTypeConfiguration<StudentGroupEntity>
    {
        public void Configure(EntityTypeBuilder<StudentGroupEntity> builder)
        {
            builder.HasKey(sg => sg.Id);

            builder.Property(sg => sg.Level)
                .HasMaxLength(10);

            builder.Property(sg => sg.PairNumber)
                .IsRequired();

            builder.HasOne(sg => sg.Student)
                .WithMany(s => s.StudentGroups)
                .HasForeignKey(sg => sg.StudentId);

            builder.HasOne(sg => sg.Group)
                .WithMany(g => g.StudentGroups)
                .HasForeignKey(sg => sg.GroupId);
        }
    }
}