using MassTransit;
using Shared.Events;
using Shared.Interfaces;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<IStockReservedEvent>
    {
        private readonly ILogger<StockReservedEventConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(ILogger<StockReservedEventConsumer> logger
            , IPublishEndpoint publishEndpoint,
            ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<IStockReservedEvent> context)
        {
            try
            {
                decimal cardsamount = 3000000m;
                decimal orderPrice = context.Message.Payment.TotalPrice;
                if (cardsamount < orderPrice)
                {
                    throw new Exception($"Amount TL on Card dont enough for order price withdraw.");
                }
                cardsamount -= orderPrice;
                var paymentCompletedEvent = new PaymentCompletedEvent(context.Message.CorrelationId) { OrderId = context.Message.OrderId };
                await _publishEndpoint.Publish(paymentCompletedEvent);
                _logger.LogInformation($"success withdraw payment on card current amount => {cardsamount} , order amount => {orderPrice}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "not enough for order");
                IPaymentFailedEvent paymentFailed = new PaymentFailedEvent(context.Message.OrderId) { Reason = "not enough for order", OrderItems= context.Message.OrderItems };
                await _publishEndpoint.Publish(paymentFailed);
            }
        }
    }
}