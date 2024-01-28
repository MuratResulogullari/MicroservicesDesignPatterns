using EventSourcing.API.DTOs;
using MediatR;

namespace EventSourcing.API.Application.Products.Commands.Update
{
    public class ChangeProductNameCommand : IRequest
  {
    public ChangeProductNameDto ChangeProductNameDto { get; set; }
  }
}