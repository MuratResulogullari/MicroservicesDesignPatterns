using MassTransit;
using Shared;

namespace Order.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<IOrderCreatedEvent>
    {
        public Task Consume(ConsumeContext<IOrderCreatedEvent> context)
        {
            
            return Task.CompletedTask;
        }
    }
}
