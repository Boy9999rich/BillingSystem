using Billing.Application.Interfaces.Repositories;
using Billing.Domain.Entities;
using Billing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Repositories
{
    public class BillingRecordRepository : IBillingRecordRepository
    {
        private readonly BillingDbContext _context;

        public BillingRecordRepository(BillingDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(BillingRecord record)
        {
            await _context.BillingRecords.AddAsync(record);
        }

        public async Task<bool> ExistsByEventIdAsync(Guid eventId)
        {
            return await _context.BillingRecords
            .AnyAsync(x => x.EventId == eventId);
        }
    }
}