namespace EventSourcing.API.DTOs
{
    public class CreateProductDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }
}
