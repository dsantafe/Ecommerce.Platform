namespace Ecommerce.Application.DTOs
{
    public class OrderDetailDTO
    {
        public int OrderDetailID { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        public string ProductName { get; set; }        
    }
}
