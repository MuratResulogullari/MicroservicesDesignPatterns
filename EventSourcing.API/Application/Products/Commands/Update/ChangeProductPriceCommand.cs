using EventSourcing.API.DTOs;
using MediatR;

namespace EventSourcing.API.Application.Products.Commands.Update
{
    public class ChangeProductPriceCommand : IRequest
  {
    public ChangeProductPriceDto ChangeProductPriceDto { get; set; }
  }
}