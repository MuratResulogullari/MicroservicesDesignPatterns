using EventSourcing.API.Application.Products.Commands.Create;
using EventSourcing.API.Application.Products.Commands.Delete;
using EventSourcing.API.Application.Products.Commands.Update;
using EventSourcing.API.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace EventSourcing.API.Presentation.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductsController : ControllerBase
  {
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost("create")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(CreateProductDto @param)
    {
      var @command = new CreateProductCommand(param);
      await _mediator.Send(@command);
      return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("change-product-name/{id?}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangeProductNameAsync(string? id, [FromBody] ChangeProductNameDto @param)
    {
      if (string.IsNullOrWhiteSpace(id))
        return NotFound();
      else
        param.Id = Guid.Parse(id);
      var @command = new ChangeProductNameCommand(param);
      await _mediator.Send(@command);
      return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpPut("change-product-price/{id?}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangeProductPriceAsync(string? id, [FromBody] ChangeProductPriceDto @param)
    {
      if (string.IsNullOrWhiteSpace(id))
        return NotFound();
      else
        param.Id = Guid.Parse(id);
      var @command = new ChangeProductPriceCommand(param);
      await _mediator.Send(@command);
      return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpDelete("delete/{id?}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync(string? id)
    {
      if (string.IsNullOrWhiteSpace(id))
        return NotFound();

      var @command = new DeleteProductCommand(Guid.Parse(id));
      await _mediator.Send(@command);
      return StatusCode(StatusCodes.Status204NoContent);
    }
  }
}