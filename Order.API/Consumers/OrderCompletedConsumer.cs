using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Interfaces;

namespace Order.API.Consumers
{
    public class OrderCompletedConsumer : IConsumer<IOrderCompletedEvent>
    {
        private readonly ILogger<OrderCompletedConsumer> _logger;
        private readonly OrderDbContext _orderDbContext;
        public OrderCompletedConsumer(ILogger<OrderCompletedConsumer> logger, OrderDbContext orderDbContext)
        {
            _logger = logger;
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<IOrderCompletedEvent> context)
        {
            var order = await _orderDbContext.Orders.FirstOrDefaultAsync(x=>x.Id==context.Message.OrderId);
            if (order != null)
            {
                order.SetStatus(OrderStatus.Completed);
                await _orderDbContext.SaveChangesAsync(); 
                _logger.LogInformation($"OrderCompletedConsumer order {context.Message.OrderId} status changed : {order.Status}");
            }
            else
            {
                _logger.LogInformation($"OrderCompletedConsumer order Id {context.Message.OrderId} not found");
            }

        }
    }
}