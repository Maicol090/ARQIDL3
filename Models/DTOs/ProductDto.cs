namespace ARQIDL3.Models.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal PriceDiscount { get; set; }
        public int Stock { get; set; }
        public string BrandName { get; set; } = null!;
        public string StoreName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public List<string> ImageUrls { get; set; } = new();
    }

}
