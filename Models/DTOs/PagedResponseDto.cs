namespace ARQIDL3.Models.DTOs
{
    public class PagedResponse<T>
    {
        public List<T> Content { get; set; } = new();
        public int TotalElements { get; set; }
        public int Number { get; set; } 
        public int Size { get; set; }
        public int TotalPages { get; set; }
    }

}
