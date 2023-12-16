using MassTransit;
using Shared;
using Shared.Events;
using Shared.Interfaces;
using Shared.Messages;

namespace WorkerService.Models
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<IOrderCreatedEvent> OrderCreatedEvent { get; set; }
        public Event<IStockReservedEvent> StockReservedEvent { get; set; }
        public Event<IStockNonReservedEvent> StockNotReservedEvent { get; set; }
        public Event<IPaymentCompletedEvent> PaymentCompletedEvent { get; set; }
        public Event<IPaymentFailedEvent> PaymentFailedEvent { get; set; }
        public Event<IOrderCompletedEvent> OrderCompletedEvent { get; set; }

        public State OrderCreated { get; private set; }
        public State StockReserved { get; private set; }
        public State StockNotReserved { get; private set; }
        public State PaymentFailed { get; private set; }
        public State PaymentCompleted { get; private set; }

        public OrderStateMachine(ILogger<OrderStateMachine> logger)
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderCreatedEvent, y => y.CorrelateBy<string>(x => x.OrderId, z => z.Message.OrderId).SelectId(context => Guid.NewGuid()));

            Event(() => StockReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

            Event(() => StockNotReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

            Event(() => PaymentCompletedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

            Event(() => PaymentFailedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

            Initially(
                When(OrderCreatedEvent)
                .Then(context =>
            {
                context.Saga.UserId = context.Message.UserId;
                context.Saga.OrderId = context.Message.OrderId;
                context.Saga.CreatedDate = DateTime.Now;
                context.Saga.SetCardInformation(context.Message.Payment.CardName, context.Message.Payment.CardNumber, context.Message.Payment.Expiration, context.Message.Payment.CVV);
                context.Saga.SetPrice(context.Message.Payment.TotalPrice);
            })
            .Then(context => { Console.WriteLine($"OrderCreatedRequestEvent before : {context.Saga}"); })
            .Publish(context => new OrderCreatedEvent(context.Saga.CorrelationId) { OrderId = context.Message.OrderId, UserId = context.Message.UserId, Payment = context.Message.Payment, OrderItems = context.Message.OrderItems })
            .TransitionTo(OrderCreated)
            .Then(context => Console.WriteLine($"OrderCreatedRequestEvent after: {context.Saga}")));

            During(OrderCreated,
                 When(StockReservedEvent)
                .TransitionTo(StockReserved)
                .Publish(context => new StockReservedEvent(context.Saga.CorrelationId)
                {
                    OrderId = context.Saga.OrderId,
                    UserId = context.Saga.UserId,
                    Payment = new PaymentMessage()
                    {
                        CardName = context.Saga.CardName,
                        CardNumber = context.Saga.CardNumber,
                        CVV = context.Saga.CVV,
                        Expiration = context.Saga.Expiration,
                        TotalPrice = context.Saga.TotalPrice
                    },
                    OrderItems = context.Message.OrderItems
                })
                .Then(context => { Console.WriteLine($"StockReservedEvent After : {context.Saga}"); }),
                 When(StockNotReservedEvent)
                .TransitionTo(StockNotReserved)
                .Publish(context => new OrderFailedEvent(context.Saga.OrderId) { Reason = context.Message.Reason })
                .Then(context => { Console.WriteLine($"OrderFailedEvent  {context.Saga}"); }));

            During(StockReserved,
                 When(PaymentCompletedEvent)
                .TransitionTo(PaymentCompleted)
                .Publish(context => new OrderCompletedEvent(context.Saga.CorrelationId) { OrderId = context.Message.OrderId })
                .Then(context => { Console.WriteLine($"OrderCompletedEvent {context.Saga}"); })
                .Finalize(),
                 When(PaymentFailedEvent)
                .TransitionTo(PaymentFailed)
                .Publish(context => new PaymentFailedEvent(context.Saga.OrderId) { Reason = context.Message.Reason })
                .Send(new Uri($"queue:{RabbitMQConsts.OrchestrationCompensableStockRollbackMessageQueueName}"), context => new CompensableStockRollbackMessage() { OrderItems = context.Message.OrderItems })
                .Then(context => { Console.WriteLine($"PaymentFailedEvent After : {context.Saga}"); }));

            SetCompletedWhenFinalized();
        }
    }
}