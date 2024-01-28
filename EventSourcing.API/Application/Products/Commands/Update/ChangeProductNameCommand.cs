using EventSourcing.API.DTOs;
using MediatR;

namespace EventSourcing.API.Application.Products.Commands.Update
{
  public class ChangeProductNameCommand : IRequest
  {
    public ChangeProductNameCommand(ChangeProductNameDto param)
    {
      Param = param;
    }

    public ChangeProductNameDto Param { get; set; }
  }
}