namespace EventSourcing.API.DTOs
{
  public class ProductPriceChangedDto
  {
    public Guid Id { get; set; }
    public decimal Price { get; set; }
  }
}