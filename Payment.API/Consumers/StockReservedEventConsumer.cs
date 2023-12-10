using MassTransit;
using Shared;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly ILogger<StockReservedEventConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(ILogger<StockReservedEventConsumer> logger
            , IPublishEndpoint publishEndpoint
            )
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var balance = 300000m;
            if (balance > context.Message.Payment.TotalPrice)
            {
                balance -= context.Message.Payment.TotalPrice;
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was withdraw from credit card for user id ={context.Message.UserId}");
                await _publishEndpoint.Publish(new PaymentCompletedEvent() { OrderId = context.Message.OrderId, UserId = context.Message.UserId });
            }
            else
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was not withdraw from credit card order payment unsuccesful orderid: {context.Message.OrderId}");
                await _publishEndpoint.Publish(new PaymentFailedEvent()
                {
                    OrderId = context.Message.OrderId,
                    UserId = context.Message.UserId,
                    Message = "Not enough balance",
                    OrderItems = context.Message.OrderItems 
                });
            }
        }
    }
}