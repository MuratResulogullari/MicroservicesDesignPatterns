using EventSourcing.API.DTOs;
using MediatR;

namespace EventSourcing.API.Application.Products.Commands.Update
{
  public class ChangeProductPriceCommand : IRequest
  {
    public ChangeProductPriceCommand(ChangeProductPriceDto changeProductPriceDto)
    {
      ChangeProductPriceDto = changeProductPriceDto;
    }

    public ChangeProductPriceDto ChangeProductPriceDto { get; set; }
  }
}