namespace Order.API.Models
{
    public class Order
    {
        public Order(string userId)
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserId = userId;
            this.CreatedDate = DateTime.Now;
        }

        public string Id { get; init; }
        public string UserId { get; init; }
        public OrderStatus Status { get; set; } = OrderStatus.Suspend!;
        public string? FailMessage { get; set; } = string.Empty!;
        public DateTime CreatedDate { get; set; }
        private List<OrderItem> _items { get; set; } = new List<OrderItem>()!;
        public IReadOnlyList<OrderItem> OrderItems => _items;
        public Address Address { get; set; }

        public void AddOrderItem(OrderItem orderItem)
        {
            if (_items.Any(x => x.Id == orderItem.Id))
            {
                throw new AggregateException($"{orderItem.Id} already exists.");
            }
            _items.Add(orderItem);
        }

        public void SetAddress(Address address)
        {
            this.Address = address;
        }

        public void SetFailMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new AggregateException($"{message} is not null or empty");
            }

            if (this.Status != OrderStatus.Fail)
            {
                throw new AggregateException($"This orders statu is {this.Status}. You must change fail after set message.");
            }
            this.FailMessage = message;
        }
    }
}