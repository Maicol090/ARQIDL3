namespace ARQIDL3.Models.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = "pendiente";

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

}
