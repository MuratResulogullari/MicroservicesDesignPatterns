using MassTransit;
using Shared;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<IOrderCreatedEvent>
    {
        public Task Consume(ConsumeContext<IOrderCreatedEvent> context)
        {
            try
            {
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}