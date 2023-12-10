using Shared;
using WorkerService.Interfaces;

namespace WorkerService.Events
{
    public class OrderCreatedRequestEvent : IOrderCreatedRequestEvent
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public PaymentMessage Payment { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}