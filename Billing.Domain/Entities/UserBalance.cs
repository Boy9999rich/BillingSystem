namespace Billing.Domain.Entities
{
    public class UserBalance
    {
        public string UserId { get; set; } = default!;

        public decimal Balance { get; set; }
    }
}
