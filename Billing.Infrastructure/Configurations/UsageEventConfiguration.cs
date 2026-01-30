using Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Infrastructure.Configurations
{
    public class UsageEventConfiguration : IEntityTypeConfiguration<UsageEvent>
    {
        public void Configure(EntityTypeBuilder<UsageEvent> builder)
        {
            builder.ToTable("UsageEvents");

            builder.HasKey(x => x.EventId);

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2);

            builder.Property(x => x.Currency)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.Timestamp)
                .IsRequired();
        }
    }
}
