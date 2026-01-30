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
            // Duplicate eventni qayta ishlamaslik
            if (await _billingRepo.ExistsByEventIdAsync(dto.EventId))
                return;

            // Eventni saqlaymiz (audit uchun)
            var usageEvent = new UsageEvent
            {
                EventId = dto.EventId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                Currency = dto.Currency,
                Timestamp = dto.Timestamp
            };

            await _usageRepo.AddAsync(usageEvent);

            // Billing yozuvi yaratamiz
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

            // Balance update
            var balance = await _balanceRepo.GetAsync(dto.UserId);

            if (balance is null)
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
        }
    }
}
