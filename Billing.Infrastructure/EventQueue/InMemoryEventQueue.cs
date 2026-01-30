using Billing.Application.DTOs;
using System.Threading.Channels;

namespace Billing.Infrastructure.EventQueue
{
    public class InMemoryEventQueue
    {
        private readonly Channel<UsageEventDto> _channel =
        Channel.CreateUnbounded<UsageEventDto>();

        public async Task PublishAsync(UsageEventDto dto)
        {
            await _channel.Writer.WriteAsync(dto);
        }

        public IAsyncEnumerable<UsageEventDto> ReadAllAsync(
            CancellationToken token)
        {
            return _channel.Reader.ReadAllAsync(token);
        }
    }
}
