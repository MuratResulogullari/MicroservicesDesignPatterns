using Shared.Interfaces;

namespace Shared.Events
{
    public class StockNonReservedEvent : IStockNonReservedEvent
    {
        public StockNonReservedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; }
        public string Reason { get; set; }

      
    }
}