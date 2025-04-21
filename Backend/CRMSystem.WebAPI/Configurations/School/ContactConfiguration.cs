using CRMSystem.WebAPI.Entities.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.WebAPI.Configurations.School
{
    public class ContactConfiguration : IEntityTypeConfiguration<ContactEntity>
    {
        public void Configure(EntityTypeBuilder<ContactEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(20);
            
            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(c => c.Person)
                .WithMany(p => p.Contacts)
                .HasForeignKey(c => c.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}