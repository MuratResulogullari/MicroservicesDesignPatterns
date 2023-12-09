namespace Shared
{
    public class OrderCreatedEvent
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public PaymentMessage Payment { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; } = new List<OrderItemMessage>()!;
    }
}