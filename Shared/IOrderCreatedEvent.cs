using MassTransit;

namespace Shared
{
    public interface IOrderCreatedEvent:CorrelatedBy<Guid>
    {
        string OrderId { get; set; }
        List<OrderItemMessage> OrderItems { get; set; }
        PaymentMessage Payment { get; set; }
        string UserId { get; set; }
    }
}