using MediatR;

namespace EventSourcing.API.Application.Products.Commands.Delete
{
  public class DeleteProductCommand:IRequest
  {
        public Guid Id { get; set; }
    }
}
