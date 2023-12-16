using MassTransit;

namespace Shared.Interfaces
{
    public interface IOrderCompletedEvent:CorrelatedBy<Guid>
    {
        public string OrderId { get; set; }
    }
}