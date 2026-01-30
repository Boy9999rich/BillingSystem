using Billing.Application.DTOs;
using Billing.Application.Services;
using Billing.Infrastructure.EventQueue;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace Billing.Infrastructure.Kafka
{
    public class UsageEventConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly InMemoryEventQueue _queue;

        public UsageEventConsumer(
            IServiceScopeFactory scopeFactory,
            InMemoryEventQueue queue)
        {
            _scopeFactory = scopeFactory;
            _queue = queue;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            await foreach (var dto in _queue.ReadAllAsync(stoppingToken))
            {
                using var scope = _scopeFactory.CreateScope();

                var billingService =
                    scope.ServiceProvider
                        .GetRequiredService<IBillingService>();

                await billingService.ProcessUsageAsync(dto);
            }
        }
    }
}
