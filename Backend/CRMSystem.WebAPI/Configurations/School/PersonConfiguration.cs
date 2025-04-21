using CRMSystem.WebAPI.Entities.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.WebAPI.Configurations.School
{
    public class PersonConfiguration : IEntityTypeConfiguration<PersonEntity>
    {
        public void Configure(EntityTypeBuilder<PersonEntity> builder)
        {
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.FullName)
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(p => p.BirthDate)
                .IsRequired();
        }
    }
}