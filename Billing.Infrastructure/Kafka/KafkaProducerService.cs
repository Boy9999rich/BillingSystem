using Billing.Application.DTOs;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Billing.Infrastructure.Kafka
{
    public class KafkaProducerService : IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;

        public KafkaProducerService(IConfiguration configuration)
        {
            var bootstrapServers = configuration["Kafka:BootstrapServers"];
            _topic = configuration["Kafka:TopicName"] ?? "usage-events";

            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                EnableIdempotence = true,
                Acks = Acks.All,
                MaxInFlight = 5,
                MessageSendMaxRetries = 3,
                CompressionType = CompressionType.Snappy,
                LingerMs = 5
            };

            _producer = new ProducerBuilder<string, string>(config)
                .Build();
        }

        
          /// <param name="dto">Usage event data</param>
        public async Task PublishAsync(UsageEventDto dto)
        {
            var message = new Message<string, string>
            {
               
                Key = dto.UserId,
                Value = JsonSerializer.Serialize(dto),
                Headers = new Headers
                {
                    { "event-type", System.Text.Encoding.UTF8.GetBytes("usage-event") },
                    { "timestamp", System.Text.Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString("o")) }
                }
            };

            try
            {
                var result = await _producer.ProduceAsync(_topic, message);

                Console.WriteLine($"Event published to Kafka: Topic={result.Topic}, Partition={result.Partition}, Offset={result.Offset}");
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Failed to publish event: {ex.Error.Reason}");
                throw;
            }
        }

        public void Dispose()
        {
            _producer?.Flush(TimeSpan.FromSeconds(10));
            _producer?.Dispose();
        }
    }
}
