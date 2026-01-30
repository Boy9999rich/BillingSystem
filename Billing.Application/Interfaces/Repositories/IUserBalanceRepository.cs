using Billing.Domain.Entities;

namespace Billing.Application.Interfaces.Repositories
{
    public interface IUserBalanceRepository
    {
        Task<UserBalance?> GetAsync(string userId);

        Task AddAsync(UserBalance balance);

        Task SaveChangesAsync();
    }
}
