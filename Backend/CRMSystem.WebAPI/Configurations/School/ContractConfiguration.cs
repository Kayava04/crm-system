using CRMSystem.WebAPI.Entities.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.WebAPI.Configurations.School
{
    public class ContractConfiguration : IEntityTypeConfiguration<ContractEntity>
    {
        public void Configure(EntityTypeBuilder<ContractEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.ContractNumber)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(c => c.PaymentAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(c => c.Student)
                .WithMany(s => s.Contracts)
                .HasForeignKey(c => c.StudentId);
        }
    }
}