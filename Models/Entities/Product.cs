using static System.Net.Mime.MediaTypeNames;

namespace ARQIDL3.Models.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal PriceDiscount { get; set; }
        public int Stock { get; set; }

        public int CategoryId { get; set; }
        public string BrandName { get; set; } = null!;
        public string StoreName { get; set; } = null!;

        // Relaciones
        public Category Category { get; set; } = null!;
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
