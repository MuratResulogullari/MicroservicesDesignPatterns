using MediatR;

namespace EventSourcing.API.Application.Products.Commands.Delete
{
  public class DeleteProductCommand:IRequest
  {
    public DeleteProductCommand(Guid ıd)
    {
      Id = ıd;
    }

    public Guid Id { get; set; }
    }
}
