namespace Billing.Domain.Entities
{
    public class UsageEvent
    {
        public Guid EventId { get; set; }

        public string UserId { get; set; } = default!;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = default!;

        public DateTime Timestamp { get; set; }
    }
}
