namespace EventSourcing.Shared.Events
{
  public class ProductCreatesEvent : IEvent
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string UserId { get; set; }
  }
}