using Billing.Application.Interfaces.Repositories;
using Billing.Domain.Entities;
using Billing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Infrastructure.Repositories
{
    public class UsageEventRepository : IUsageEventRepository
    {
        private readonly BillingDbContext _context;

        public UsageEventRepository(BillingDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UsageEvent usageEvent)
        {
            await _context.UsageEvents.AddAsync(usageEvent);
        }

        public async Task<bool> ExistsAsync(Guid eventId)
        {
            return await _context.UsageEvents
            .AnyAsync(x => x.EventId == eventId);
        }
    }
}
