namespace Billing.Application.DTOs
{
    public class UsageEventDto
    {
        public Guid EventId { get; set; }

        public string UserId { get; set; } 

        public decimal Amount { get; set; }

        public string Currency { get; set; } 

        public DateTime Timestamp { get; set; }
    }
}
