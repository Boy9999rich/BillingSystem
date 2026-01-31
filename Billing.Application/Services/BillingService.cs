using Billing.Application.DTOs;
using Billing.Application.Interfaces.Repositories;
using Billing.Domain.Entities;

namespace Billing.Application.Services
{
    public class BillingService : IBillingService
    {
        private readonly IUsageEventRepository _usageRepo;
        private readonly IBillingRecordRepository _billingRepo;
        private readonly IUserBalanceRepository _balanceRepo;

        public BillingService(IUsageEventRepository usageRepo, IBillingRecordRepository billingRepo, IUserBalanceRepository balanceRepo)
        {
            _usageRepo = usageRepo;
            _billingRepo = billingRepo;
            _balanceRepo = balanceRepo;
        }

        public async Task<UserBalanceDto> GetBalanceAsync(string userId)
        {
            var balance = await _balanceRepo.GetAsync(userId);

            return new UserBalanceDto
            {
                UserId = userId,
                Balance = balance?.Balance ?? 0
            };
        }

        public async Task ProcessUsageAsync(UsageEventDto dto)
        {
            if (await _billingRepo.ExistsByEventIdAsync(dto.EventId))
            {
                Console.WriteLine($"Duplicate event ignored: {dto.EventId}");
                return; 
            }
            var usageEvent = new UsageEvent
            {
                EventId = dto.EventId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                Currency = dto.Currency,
                Timestamp = dto.Timestamp
            };
            await _usageRepo.AddAsync(usageEvent);

            
            var billingRecord = new BillingRecord
            {
                BillingId = Guid.NewGuid(),
                EventId = dto.EventId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                Currency = dto.Currency,
                ProcessedAt = DateTime.UtcNow
            };
            await _billingRepo.AddAsync(billingRecord);

            // 4. User balance yangilash
            var balance = await _balanceRepo.GetAsync(dto.UserId);

            if (balance == null)
            {
                
                balance = new UserBalance
                {
                    UserId = dto.UserId,
                    Balance = 0
                };
                await _balanceRepo.AddAsync(balance);
            }

            
            balance.Balance += dto.Amount;

            
            await _balanceRepo.SaveChangesAsync();

            Console.WriteLine($"Successfully processed event {dto.EventId} for user {dto.UserId}. New balance: {balance.Balance}");
        }
    }
}
