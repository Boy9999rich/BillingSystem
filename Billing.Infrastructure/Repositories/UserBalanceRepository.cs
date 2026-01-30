using Billing.Application.Interfaces.Repositories;
using Billing.Domain.Entities;
using Billing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Repositories
{
    public class UserBalanceRepository : IUserBalanceRepository
    {
        private readonly BillingDbContext _context;

        public UserBalanceRepository(BillingDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserBalance balance)
        {
            await _context.UserBalances.AddAsync(balance);
        }

        public async Task<UserBalance?> GetAsync(string userId)
        {
            return await _context.UserBalances
            .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
