using EventSourcing.API.Domain.EventStores;
using MediatR;

namespace EventSourcing.API.Application.Products.Commands.Create
{
  public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
  {
    private readonly ProductStream _productStream;

    public CreateProductCommandHandler(ProductStream productStream)
    {
      _productStream = productStream;
    }

    public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
      _productStream.Created(request.CreateProductDto);
      await _productStream.SaveAsync();
      return Unit.Value;
    }
  }
}