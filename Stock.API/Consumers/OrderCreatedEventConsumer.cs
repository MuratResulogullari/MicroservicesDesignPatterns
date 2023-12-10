using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Stock.API.Models;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly StockDbContext _stockDbContext;
        private readonly ILogger<OrderCreatedEventConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint; // Birden fazla microservices dinleyebilir one to many
        private readonly ISendEndpointProvider _sendEndpointProvider; //Kuyruğu belirtiyorsun kuyruğu dinleyen yolluyorsun one to one

        public OrderCreatedEventConsumer(StockDbContext stockDbContext
            , ILogger<OrderCreatedEventConsumer> logger
            , IPublishEndpoint publishEndpoint
            , ISendEndpointProvider sendEndpointProvider
            )
        {
            _stockDbContext = stockDbContext;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();
            foreach (var item in context.Message.OrderItems)
            {
                stockResult.Add(await _stockDbContext.Stocks.AnyAsync(x => x.ProductId == item.ProductId && x.Count > item.Count));
            }
            if (stockResult.All(x => x.Equals(true)))
            {
                foreach (var item in context.Message.OrderItems)
                {
                    var stock = await _stockDbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);
                    if (stock != null)
                    {
                        stock.Count -= item.Count;
                        _logger.LogInformation($"ProductId: {item.ProductId}  has {item.Count} count stock was reserved for UserId: {context.Message.UserId}");
                    }
                }
                await _stockDbContext.SaveChangesAsync();
                var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQConsts.PaymentStockReservedEventQueueName}"));
                StockReservedEvent stockReservedEvent = new StockReservedEvent()
                {
                    OrderId = context.Message.OrderId,
                    UserId = context.Message.UserId,
                    Payment = context.Message.Payment,
                    OrderItems = context.Message.OrderItems,
                };
                await sendEndpoint.Send(stockReservedEvent);
            }
            else
            {
                await _publishEndpoint.Publish(new StockNonReservedEvent()
                {
                    OrderId = context.Message.OrderId,
                    Message = $"Not enough stock"
                });
                _logger.LogError($"Not enough stock for OrderId: {context.Message.OrderId} ");
            }
        }
    }
}