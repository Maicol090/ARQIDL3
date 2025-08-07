namespace ARQIDL3.Models.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal Total { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = new();
    }
    public class OrderDetailDto
    {
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }

}
