namespace ARQIDL3.Models.DTOs
{
    public class ProductFilterDto
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? CategoryId { get; set; }
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }

}
