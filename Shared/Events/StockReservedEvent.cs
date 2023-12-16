using Shared.Interfaces;

namespace Shared.Events
{
    public class StockReservedEvent : IStockReservedEvent
    {
        public StockReservedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; }
        public string UserId { get; set; }
        public PaymentMessage Payment { get; set; }
        public string OrderId { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}