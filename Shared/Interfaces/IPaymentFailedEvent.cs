using MassTransit;

namespace Shared.Interfaces
{
    public interface IPaymentFailedEvent :CorrelatedBy<Guid>
    {
        string OrderId { get; set; }
        string Reason { get; set; }
        List<OrderItemMessage> OrderItems { get; set; }
    }
}