using EventSourcing.API.DTOs;
using EventSourcing.Shared.Events;
using EventStore.ClientAPI;

namespace EventSourcing.API.Domain.EventStores
{
  public class ProductStream : AbstractStream
  {
    public static string StreamName => "ProductStream";

    public ProductStream(IEventStoreConnection eventStoreConnection) : base(StreamName, eventStoreConnection)
    {
    }

    public void Created(CreateProductDto createProductDto)
    {
      Events.AddLast(new ProductCreatedEvent()
      {
        Id = Guid.NewGuid(),
        UserId = createProductDto.UserId,
        Name = createProductDto.Name,
        Price = createProductDto.Price,
        Stock = createProductDto.Stock
      });
    }

    public void NameChanged(ChangeProductNameDto changeProductNameDto) =>
      Events.AddLast(new ProductNameChangedEvent() { Id = changeProductNameDto.Id, Name = changeProductNameDto.Name });

    public void PriceChanged(ChangeProductPriceDto changeProductPriceDto) =>
      Events.AddLast(new ProductPriceChangedEvent() { Id = changeProductPriceDto.Id, Price = changeProductPriceDto.Price });

    public void Deleted(Guid id) =>
      Events.AddLast(new ProductDeletedEvent() { Id = id });
  }
}