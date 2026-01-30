using Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billing.Infrastructure.Configurations
{
    public class UserBalanceConfiguration : IEntityTypeConfiguration<UserBalance>
    {
        public void Configure(EntityTypeBuilder<UserBalance> builder)
        {
            builder.ToTable("UserBalances");

            builder.HasKey(x => x.UserId);

            builder.Property(x => x.UserId)
                .HasMaxLength(100);

            builder.Property(x => x.Balance)
                .HasPrecision(18, 2); ;
        }
    }
}
