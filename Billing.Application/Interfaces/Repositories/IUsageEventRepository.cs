using Billing.Domain.Entities;

namespace Billing.Application.Interfaces.Repositories
{
    public interface IUsageEventRepository
    {
        Task<bool> ExistsAsync(Guid eventId);

        Task AddAsync(UsageEvent usageEvent);
    }
}
