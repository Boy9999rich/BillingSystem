using Billing.Application.DTOs;
using Billing.Application.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using static Confluent.Kafka.ConfigPropertyNames;
using static System.Formats.Asn1.AsnWriter;

namespace Billing.Infrastructure.Kafka
{
    public class UsageEventConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly string _topic;
        private readonly ConsumerConfig _consumerConfig;

        public UsageEventConsumer(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _topic = _configuration["Kafka:TopicName"] ?? "usage-events";

            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = _configuration["Kafka:GroupId"] ?? "billing-consumer-group",

                AutoOffsetReset = AutoOffsetReset.Earliest, 

                EnableAutoCommit = false,

                SessionTimeoutMs = 45000,
                MaxPollIntervalMs = 300000,

                FetchMinBytes = 1,
                FetchWaitMaxMs = 500
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => ConsumeMessages(stoppingToken), stoppingToken);
        }

        private void ConsumeMessages(CancellationToken stoppingToken)
        {
            using var consumer = new ConsumerBuilder<string, string>(_consumerConfig)
                .SetErrorHandler((_, e) => Console.WriteLine($"Kafka error: {e.Reason}"))
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    Console.WriteLine($"Partitions assigned: {string.Join(", ", partitions)}");
                })
                .Build();

            consumer.Subscribe(_topic);
            Console.WriteLine($"Kafka consumer started. Listening to topic: {_topic}");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);

                        if (consumeResult?.Message == null)
                            continue;

                        Console.WriteLine($"Received message: Partition={consumeResult.Partition}, Offset={consumeResult.Offset}");

                        var usageEvent = JsonSerializer.Deserialize<UsageEventDto>(
                            consumeResult.Message.Value,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );

                        if (usageEvent == null)
                        {
                            Console.WriteLine("Failed to deserialize message");
                            consumer.Commit(consumeResult); 
                            continue;
                        }

                        
                        ProcessEvent(usageEvent).Wait();

                        consumer.Commit(consumeResult);

                        Console.WriteLine($"Successfully processed event: {usageEvent.EventId}");
                    }
                    catch (ConsumeException ex)
                    {
                        Console.WriteLine($"Consume error: {ex.Error.Reason}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Processing error: {ex.Message}");
                        
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Consumer cancelled");
            }
            finally
            {
                consumer.Close();
                Console.WriteLine("Kafka consumer stopped");
            }
        }

        private async Task ProcessEvent(UsageEventDto usageEvent)
        {
            using var scope = _scopeFactory.CreateScope();

            var billingService = scope.ServiceProvider
                .GetRequiredService<IBillingService>();

            await billingService.ProcessUsageAsync(usageEvent);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping Kafka consumer...");
            await base.StopAsync(cancellationToken);
        }
    }
}
