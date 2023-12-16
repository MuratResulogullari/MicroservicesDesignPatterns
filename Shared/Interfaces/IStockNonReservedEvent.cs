using MassTransit;

namespace Shared.Interfaces
{
    public interface IStockNonReservedEvent : CorrelatedBy<Guid>
    {
        string Reason { get; set; }
    }
}