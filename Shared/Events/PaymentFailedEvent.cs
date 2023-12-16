using Shared.Interfaces;

namespace Shared.Events
{
    public class PaymentFailedEvent : IPaymentFailedEvent
    {
        public PaymentFailedEvent(string orderId)
        {
            OrderId = orderId;
        }
        public string OrderId { get ; set ; }
        public string Reason { get ; set ; }
        public List<OrderItemMessage> OrderItems { get; set; }
        public Guid CorrelationId { get; }
    }
}