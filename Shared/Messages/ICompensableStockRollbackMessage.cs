namespace Shared.Messages
{
    public interface ICompensableStockRollbackMessage
    {
        List<OrderItemMessage> OrderItems { get; set; }
    }
}