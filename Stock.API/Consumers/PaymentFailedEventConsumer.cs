using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Stock.API.Models;

namespace Stock.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly StockDbContext _stockDbContext;
        private readonly ILogger<PaymentFailedEventConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint; // Birden fazla microservices dinleyebilir one to many
        private readonly ISendEndpointProvider _sendEndpointProvider; //Kuyruğu belirtiyorsun kuyruğu dinleyen yolluyorsun one to one

        public PaymentFailedEventConsumer(StockDbContext stockDbContext
            , ILogger<PaymentFailedEventConsumer> logger
            , IPublishEndpoint publishEndpoint
            , ISendEndpointProvider sendEndpointProvider
            )
        {
            _stockDbContext = stockDbContext;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            foreach (var item in context.Message.OrderItems)
            {
                var stock = await _stockDbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);
                if (stock != null)
                {
                    stock.Count += item.Count;

                    _logger.LogInformation($"{stock.Id} count rollback");
                }
            }
            await _stockDbContext.SaveChangesAsync();
            _logger.LogError(context.Message.Message.ToString());
        }
    }
}