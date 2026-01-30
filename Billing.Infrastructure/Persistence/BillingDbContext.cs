using Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Persistence
{
    public class BillingDbContext : DbContext
    {
        public BillingDbContext(DbContextOptions<BillingDbContext> options)
        : base(options)
        {
        }

        public DbSet<UsageEvent> UsageEvents => Set<UsageEvent>();
        public DbSet<BillingRecord> BillingRecords => Set<BillingRecord>();
        public DbSet<UserBalance> UserBalances => Set<UserBalance>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BillingDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
