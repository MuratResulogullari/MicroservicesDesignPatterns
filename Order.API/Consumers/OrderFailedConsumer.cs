using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Interfaces;

namespace Order.API.Consumers
{
    public class OrderFailedConsumer : IConsumer<IOrderFailedEvent>
    {
        private readonly ILogger<OrderCompletedConsumer> _logger;
        private readonly OrderDbContext _orderDbContext;

        public OrderFailedConsumer(ILogger<OrderCompletedConsumer> logger, OrderDbContext orderDbContext)
        {
            _logger = logger;
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<IOrderFailedEvent> context)
        {
            var order = await _orderDbContext.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);
            if (order != null)
            {
                order.SetStatus(OrderStatus.Fail);
                order.SetFailMessage(context.Message.Reason);
                await _orderDbContext.SaveChangesAsync();
             _logger.LogInformation($"OrderFailedConsumer order {context.Message.OrderId} status changed : {order.Status}");
            }
            else
            {
                _logger.LogInformation($"OrderFailedConsumer order Id {context.Message.OrderId} not found");
            }
        }
    }
}