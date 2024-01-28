using EventSourcing.API.DTOs;
using MediatR;

namespace EventSourcing.API.Application.Products.Commands.Create
{
    public class CreateProductCommand : IRequest
    {
        public CreateProductDto CreateProductDto { get; set; }
    }
}
