using Shared.Interfaces;

namespace Shared.Events
{
    public class OrderFailedEvent : IOrderFailedEvent
    {
        public OrderFailedEvent(string orderId)
        {
            OrderId = orderId;
        }

        public string OrderId { get; set; }
        public string Reason { get; set; }
    }
}