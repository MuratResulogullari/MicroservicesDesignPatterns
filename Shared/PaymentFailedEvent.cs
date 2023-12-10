namespace Shared
{
    public class PaymentFailedEvent
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; } = new List<OrderItemMessage>()!;
    }
}