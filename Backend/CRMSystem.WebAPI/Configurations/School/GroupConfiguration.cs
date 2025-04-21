using CRMSystem.WebAPI.Entities.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.WebAPI.Configurations.School
{
    public class GroupConfiguration : IEntityTypeConfiguration<GroupEntity>
    {
        public void Configure(EntityTypeBuilder<GroupEntity> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.GroupName)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(g => g.Language)
                .WithMany(l => l.Groups)
                .HasForeignKey(g => g.LanguageId);
        }
    }
}