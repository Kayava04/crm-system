using CRMSystem.WebAPI.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.WebAPI.Configurations.Auth
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);
        
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Property(u => u.BirthDate)
                .IsRequired();
        
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
        
            builder.HasIndex(u => u.Email)
                .IsUnique();
        
            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
        
            builder.HasIndex(u => u.Username)
                .IsUnique();

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);
        
            builder.Property(u => u.CreatedAt)
                .IsRequired();
            
            builder.Property(u => u.UpdatedAt)
                .IsRequired();
        
            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}