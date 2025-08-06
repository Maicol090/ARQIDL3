namespace ARQIDL3.Models.DTOs
{
    public class OrderCreateDto
    {
        public List<OrderDetailCreateDto> Products { get; set; } = new();
    }
    public class OrderDetailCreateDto
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal PriceDiscount { get; set; }
        public int Quantity { get; set; }
    }

}
