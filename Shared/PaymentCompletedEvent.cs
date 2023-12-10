namespace Shared
{
    public class PaymentCompletedEvent
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
    }
}