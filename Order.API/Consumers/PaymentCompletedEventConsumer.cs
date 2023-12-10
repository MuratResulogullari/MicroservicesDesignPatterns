using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared;

namespace Order.API.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly ILogger<PaymentCompletedEventConsumer> _logger;
        private readonly OrderDbContext _orderDbContext;

        public PaymentCompletedEventConsumer(ILogger<PaymentCompletedEventConsumer> logger
            , OrderDbContext orderDbContext
            )
        {
            _logger = logger;
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var order = await _orderDbContext.Orders.FirstOrDefaultAsync(x=>x.Id==context.Message.OrderId);
            if (order != null)
            {
                order.SetStatus(OrderStatus.Completed);
                _orderDbContext.SaveChanges();
                _logger.LogInformation($"order {order.Id} completed.");
            }
            else
            {
                _logger.LogError($"order {order.Id} not found.");
            }
        }
    }
}
