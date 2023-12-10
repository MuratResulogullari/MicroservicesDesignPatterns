namespace Stock.API.Models
{
    public class Stock
    {
        public Stock(string productId,int count)
        {
            this.Id=Guid.NewGuid().ToString();
            this.ProductId = productId;
            this.Count = count;

        }
        public string Id { get; set; }
        public string ProductId { get; set; }
        public int Count { get; set; }
    }
}
