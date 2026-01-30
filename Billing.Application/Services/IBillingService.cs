using Billing.Application.DTOs;

namespace Billing.Application.Services
{
    public interface IBillingService
    {
        Task ProcessUsageAsync(UsageEventDto usageEvent);

        Task<UserBalanceDto> GetBalanceAsync(string userId);
    }
}
