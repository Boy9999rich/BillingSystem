using Billing.Application.DTOs;

namespace Billing.Infrastructure.Kafka
{
    public interface IKafkaProducer
    {
        Task PublishUsageEventAsync(UsageEventDto dto);
    }
}
