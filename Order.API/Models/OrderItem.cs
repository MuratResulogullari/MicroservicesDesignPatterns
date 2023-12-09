namespace Order.API.Models
{
    public class OrderItem
    {
        public OrderItem(string OrderId, string ProductId, string ProductName, string ProductPrice, int Count, decimal Price)
        {
            this.Id = Guid.NewGuid().ToString();
            this.OrderId = OrderId;
            this.ProductId = ProductId;
            this.ProductName = ProductName;
            this.ProductPrice = ProductPrice;
            this.Count = Count;
            this.Price = Price;
        }

        public string Id { get; init; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}