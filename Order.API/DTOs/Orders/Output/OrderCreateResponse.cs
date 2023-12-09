namespace Order.API.DTOs.Orders.Output
{
    public class OrderCreateResponse
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}