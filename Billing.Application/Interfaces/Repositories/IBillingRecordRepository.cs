using Billing.Domain.Entities;

namespace Billing.Application.Interfaces.Repositories
{
    public interface IBillingRecordRepository
    {
        Task<bool> ExistsByEventIdAsync(Guid eventId);

        Task AddAsync(BillingRecord record);
    }
}
