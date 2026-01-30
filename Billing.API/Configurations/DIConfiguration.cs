using Billing.Application.Interfaces.Repositories;
using Billing.Application.Services;
using Billing.Infrastructure.EventQueue;
using Billing.Infrastructure.Kafka;
using Billing.Infrastructure.Repositories;

namespace Billing.API.Configurations
{
    public static class DIConfiguration
    {
        public static void ConfigureDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IBillingRecordRepository, BillingRecordRepository>();
            builder.Services.AddScoped<IUserBalanceRepository, UserBalanceRepository>();
            builder.Services.AddScoped<IUsageEventRepository, UsageEventRepository>();
            builder.Services.AddScoped<IBillingService, BillingService>();
            builder.Services.AddHostedService<UsageEventConsumer>();
            builder.Services.AddSingleton<InMemoryEventQueue>();
            builder.Services.AddHostedService<UsageEventConsumer>();
        }
    }
}
