using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared;

namespace Order.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly ILogger<PaymentFailedEventConsumer> _logger;
        private readonly OrderDbContext _orderDbContext;

        public PaymentFailedEventConsumer(ILogger<PaymentFailedEventConsumer> logger
            , OrderDbContext orderDbContext
            )
        {
            _logger = logger;
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var order = await _orderDbContext.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);
            if (order != null)
            {
                 order.SetStatus(OrderStatus.Fail);
                 order.SetFailMessage(context.Message.Message.Trim());
                _orderDbContext.SaveChanges();
                _logger.LogInformation($"order {order.Id} failed.");
            }
            else
            {
                _logger.LogError($"order {order.Id} not found.");
            }
            _logger.LogError(context.Message.Message.ToString());
        }
    }
}