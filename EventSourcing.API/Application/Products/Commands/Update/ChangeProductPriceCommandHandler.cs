using EventSourcing.API.Domain.EventStores;
using MediatR;

namespace EventSourcing.API.Application.Products.Commands.Update
{
  public class ChangeProductPriceCommandHandler : IRequestHandler<ChangeProductPriceCommand>
  {
    private readonly ProductStream _productStream;

    public ChangeProductPriceCommandHandler(ProductStream productStream)
    {
      _productStream = productStream;
    }

    public async Task<Unit> Handle(ChangeProductPriceCommand request, CancellationToken cancellationToken)
    {
      _productStream.PriceChanged(request.ChangeProductPriceDto);
      await _productStream.SaveAsync();
      return Unit.Value;
    }
  }
}