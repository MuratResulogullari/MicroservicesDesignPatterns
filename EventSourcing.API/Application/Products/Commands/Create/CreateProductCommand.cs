using EventSourcing.API.DTOs;
using MediatR;

namespace EventSourcing.API.Application.Products.Commands.Create
{
    public class CreateProductCommand : IRequest
    {
    public CreateProductCommand(CreateProductDto createProductDto)
    {
      CreateProductDto = createProductDto;
    }

    public CreateProductDto CreateProductDto { get; set; }
    }
}
