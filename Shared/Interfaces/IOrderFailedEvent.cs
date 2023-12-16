namespace Shared.Interfaces
{
    public interface IOrderFailedEvent
    {
        string OrderId { get; set; }
        string Reason { get; set; }
    }
}