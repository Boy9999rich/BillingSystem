using Billing.Application.Interfaces.Repositories;
using Billing.Application.Services;
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
            builder.Services.AddScoped<KafkaProducerService>();
            builder.Services.AddHostedService<UsageEventConsumer>();
        }
    }
}
