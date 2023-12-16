using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using Shared.Interfaces;
using Stock.API.Models;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<IOrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedEventConsumer> _logger;
        private readonly StockDbContext _stockDbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public OrderCreatedEventConsumer(ILogger<OrderCreatedEventConsumer> logger
            , StockDbContext stockDbContext
            , IPublishEndpoint publishEndpoint
            , ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _stockDbContext = stockDbContext;
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task Consume(ConsumeContext<IOrderCreatedEvent> context)
        {
            try
            {
                List<bool> stockResult = new List<bool>();
                foreach (var item in context.Message.OrderItems)
                {
                    stockResult.Add(await _stockDbContext.Stocks.AnyAsync(x => x.ProductId == item.ProductId && x.Count > item.Count));
                }
                if (stockResult.All(x => !x.Equals(true)))
                {
                    throw new Exception();
                }
                foreach (var item in context.Message.OrderItems)
                {
                    var stock = await _stockDbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                    if (stock != null)
                    {
                        stock.Count += item.Count;
                    }
                }

                await _stockDbContext.SaveChangesAsync();
                _logger.LogInformation($"Stock was reserved for CorrelationId : {context.Message.CorrelationId}");

                Shared.Interfaces.IStockReservedEvent stockReservedEvent = new Shared.Events.StockReservedEvent(context.Message.CorrelationId)
                {
                    OrderId = context.Message.OrderId,
                    Payment = context.Message.Payment,
                    UserId = context.Message.UserId,
                };

                await _publishEndpoint.Publish(stockReservedEvent);
            }
            catch (Exception ex)
            {
                IOrderFailedEvent orderFailedEvent = new OrderFailedEvent(context.Message.OrderId)
                {
                    Reason = "Not enough stock"
                };
                await _publishEndpoint.Publish(orderFailedEvent);
                _logger.LogInformation(ex, $"Stock was not reserved for  OrderId {context.Message.OrderId} ", context.Message);
            }
        }
    }
}