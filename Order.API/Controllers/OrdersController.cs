using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.API.DTOs.Orders.Input;
using Order.API.DTOs.Orders.Output;
using Order.API.Models;
using Shared;
using Shared.Events;
using Shared.Interfaces;
using System.Net.Mime;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrdersController(OrderDbContext context

            , ISendEndpointProvider sendEndpointProvider
            , IPublishEndpoint publishEndpoint
            )
        {
            _context = context;

            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost("create"), DisableRequestSizeLimit]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(OrderCreateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] OrderCreateRequest request)
        {
            request.UserId = Guid.NewGuid().ToString();
            if (request == null || string.IsNullOrEmpty(request.UserId) || request.OrderItems == null || request.OrderItems.Count == 0)
            {
                return BadRequest("Invalid request");
            }
            try
            {
                // Convert DTOs to entities and save to the database
                var order = new Models.Order(request.UserId);

                request.OrderItems.ForEach(item => order.AddOrderItem(new OrderItem(order.Id, item.ProductId, item.ProductName, item.Count, item.Price)));

                order.SetAddress(new Address(request.Address.Province, request.Address.City, request.Address.District, request.Address.Street, request.Address.Text, request.Address.Phone, request.Address.ZipCode));

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                var orderCreatedEvent = new OrderCreatedEvent(Guid.NewGuid())
                {
                    OrderId = order.Id,
                    UserId = order.UserId,
                    Payment = new PaymentMessage
                    {
                        CardName = request.Payment.CardName,
                        CardNumber = request.Payment.CardNumber,
                        CVV = request.Payment.CVV,
                        Expiration = request.Payment.Expiration,
                        TotalPrice = request.OrderItems.Sum(x => x.Count * x.Price),
                    },
                    OrderItems = request.OrderItems.Select(x => new OrderItemMessage() { ProductId = x.ProductId, Count = x.Count, Price = x.Price }).ToList()
                };
                var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQConsts.OrchestrationOrderSagaQueueName}"));
                await sendEndpoint.Send<IOrderCreatedEvent>(orderCreatedEvent); // Send ile  Rabbitmq kuyruğuna  göndermiş olursun , kuyruktaki bilgiler kayıt eder ve   subscribe olanlar bu evente alır
                                                                                // Microservice in bir kuyruğu olur ve diğer microservislerden bazıları sadece bu kuyruğa subscribe olur.
                                                                                //await _publishEndpoint.Publish(orderCreatedRequestEvent);
                                                                                //await _publishEndpoint.Publish<IOrderCreatedEvent>(orderCreatedEvent);
                var response = new OrderCreateResponse
                {
                    OrderId = order.Id,
                    UserId = order.UserId,
                    CreatedDate = DateTime.UtcNow,
                };

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in CreateAsync: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request");
            }
        }
    }
}