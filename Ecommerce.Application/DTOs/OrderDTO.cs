namespace Ecommerce.Application.DTOs
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
    }
}
