using MassTransit;

namespace Shared.Interfaces
{
    public interface IStockReservedEvent : CorrelatedBy<Guid>
    {
        string UserId { get; set; }
        string OrderId { get; set; }
        PaymentMessage Payment { get; set; }
        List<OrderItemMessage> OrderItems { get; set; }
    }
}