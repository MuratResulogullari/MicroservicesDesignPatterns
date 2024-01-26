namespace EventSourcing.Shared.Events
{
  public class ProductPriceChangedEvent : IEvent

  {
    public Guid Id { get; set; }
    public decimal Price { get; set; }
  }
}