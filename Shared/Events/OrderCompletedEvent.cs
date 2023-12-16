using Shared.Interfaces;

namespace Shared.Events
{
    public class OrderCompletedEvent : IOrderCompletedEvent
    {
        public OrderCompletedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }


        public Guid CorrelationId { get; }
        public string OrderId { get; set; }
    }
}