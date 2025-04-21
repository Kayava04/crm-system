using CRMSystem.WebAPI.Entities.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.WebAPI.Configurations.Email
{
    public class MessageConfiguration : IEntityTypeConfiguration<MessageEntity>
    {
        public void Configure(EntityTypeBuilder<MessageEntity> builder)
        {
            builder.HasKey(m => m.Id);
            
            builder.Property(m => m.ReceiverId)
                .IsRequired();
            
            builder.Property(m => m.Subject)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(m => m.Body)
                .IsRequired();

            builder.Property(m => m.Email)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(m => m.SentAt)
                .IsRequired();
            
            builder.HasIndex(m => m.ReceiverId);
            builder.HasIndex(m => m.Email);
        }
    }
}