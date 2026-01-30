using Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billing.Infrastructure.Configurations
{
    public class BillingRecordConfiguration : IEntityTypeConfiguration<BillingRecord>
    {
        public void Configure(EntityTypeBuilder<BillingRecord> builder)
        {
            builder.ToTable("BillingRecords");

            builder.HasKey(x => x.BillingId);

            builder.HasIndex(x => x.EventId)
                .IsUnique();

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2);

            builder.Property(x => x.Currency)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.ProcessedAt)
                .IsRequired();
        }
    }
}
