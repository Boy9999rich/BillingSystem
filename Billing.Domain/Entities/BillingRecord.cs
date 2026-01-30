namespace Billing.Domain.Entities
{
    public class BillingRecord
    {
        public Guid BillingId { get; set; }

        public Guid EventId { get; set; }

        public string UserId { get; set; } = default!;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = default!;

        public DateTime ProcessedAt { get; set; }
    }
}
