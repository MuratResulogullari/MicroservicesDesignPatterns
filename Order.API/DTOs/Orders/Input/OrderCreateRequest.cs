namespace Order.API.DTOs.Orders.Input
{
    public class OrderCreateRequest
    {
        public string UserId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public PaymentDto Payment { get; set; }
        public AddressDto Address { get; set; }
    }

    public class OrderItemDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }

    public class PaymentDto
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
    }

    public class AddressDto
    {
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string Text { get; set; }
        public string? Phone { get; set; }
        public string? ZipCode { get; set; }
    }
}