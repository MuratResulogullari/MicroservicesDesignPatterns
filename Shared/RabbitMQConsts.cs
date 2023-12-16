namespace Shared
{
    public class RabbitMQConsts
    {
        // Choreography
        public const string StockOrderCreatedEventQueueName = "stock-order-created-queue";

        //public const string StockPaymentFailedEventQueueName = "stock-payment-failed-queue";
        //public const string OrderStockNotReservedEventQueueName = "order-stock-not-reserved-queue";
        //public const string OrderPaymentCompletedEventQueueName = "order-payment-completed-queue";
        //public const string OrderPaymentFailedEventQueueName = "order-payment-failed-queue";
        //public const string PaymentStockReservedEventQueueName = "payment-stock-reserved-queue";

        //Orchestration
        public const string OrchestrationOrderSagaQueueName = "orchestration-order-saga-queue";
        public const string OrchestrationCompensableStockRollbackMessageQueueName = "orchestration-compensable-stock-rollback-message-queue";
        public const string OrchestrationOrderCreatedQueueName = "orchestration-order-created-queue";
        public const string OrchestrationOrderCompletedQueueName = "orchestration-order-completed-queue";
        public const string OrchestrationOrderFailedQueueName = "orchestration-order-failed-queue";
        public const string OrchestrationStockReservedQueueName = "orchestration-stock-reserved-queue";
        public const string OrchestrationPaymentCompletedQueueName = "orchestration-payment-completed-queue";
    }
}