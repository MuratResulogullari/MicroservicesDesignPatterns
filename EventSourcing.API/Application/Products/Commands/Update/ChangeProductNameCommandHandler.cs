using EventSourcing.API.Domain.EventStores;
using MediatR;

namespace EventSourcing.API.Application.Products.Commands.Update
{
  public class ChangeProductNameCommandHandler : IRequestHandler<ChangeProductNameCommand>
  {
    private readonly ProductStream _productStream;

    public ChangeProductNameCommandHandler(ProductStream productStream)
    {
      _productStream = productStream;
    }

    public async Task<Unit> Handle(ChangeProductNameCommand request, CancellationToken cancellationToken)
    {
      _productStream.NameChanged(request.Param);
      await _productStream.SaveAsync();
      return Unit.Value;
    }
  }
}