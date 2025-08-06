namespace ARQIDL3.Models.Entities
{
    public class Image
    {
        public int ImageId { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; } = null!;

        public Product Product { get; set; } = null!;
    }

}
