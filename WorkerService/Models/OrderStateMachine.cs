using Automatonymous;
using Shared;
using WorkerService.Interfaces;

namespace WorkerService.Models
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        private readonly ILogger<OrderStateMachine> _logger;    
        public OrderStateMachine(ILogger<OrderStateMachine> logger )
        {
            _logger = logger;
            this.InstanceState(x => x.CurrentState);
            // Veritabanında event ten gelen OrderId var mı ?  yoksa yeni bir OrderStateInstance oluştur ve CorrelateId ise Guid.NewGuid() metodundan turesin
            Event(() => OrderCreatedRequestEvent, y => y.CorrelateBy<string>(x => x.OderderId, z => z.Message.OrderId).SelectId(context => Guid.NewGuid()));

            Initially(When(OrderCreatedRequestEvent).Then(context =>
            {
                context.Instance.UserId = context.Data.UserId;
                context.Instance.OderderId = context.Data.OrderId;
                context.Instance.CreatedDate = DateTime.Now;
                context.Instance.SetCardInformation(context.Data.Payment.CardName, context.Data.Payment.CardNumber, context.Data.Payment.Expiration, context.Data.Payment.CVV);
                context.Instance.SetPrice(context.Data.Payment.TotalPrice);
            }).Then(context =>
            {
               _logger.LogInformation($"OrderCreatedRequestEvent before: {context.Instance}");
            }).Publish(context=> new OrderCreatedEvent(context.Instance.CorrelationId) {OrderItems = context.Data.OrderItems })
            .TransitionTo(OrderCreated).Then(context =>
            {
                _logger.LogInformation($"OrderCreatedRequestEvent after: {context.Instance}");
            }));
        }

        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }
        public State OrderCreated { get; private set; }
    }
}