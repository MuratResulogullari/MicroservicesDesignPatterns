using MassTransit;
using System.Text;

namespace WorkerService.Models
{
    public class OrderStateInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }

        public void SetCardInformation(string cardName, string cardNumber, string expiration, string cvv)
        {
            this.CardName = cardName;
            this.CardNumber = cardNumber;
            this.Expiration = expiration;
            this.CVV = cvv;
        }

        public void SetPrice(decimal price)
        {
            this.TotalPrice = price;
        }

        public override string ToString()
        {
            var properties = GetType().GetProperties().ToList();
            StringBuilder stringBuilder = new StringBuilder();
            properties.ForEach(p =>
            {
                var value = p.GetValue(this, null);
                stringBuilder.AppendLine($"{p.Name}:{value}");
            });
            stringBuilder.AppendLine("-----------------------------------------");
            return stringBuilder.ToString();
        }
    }
}