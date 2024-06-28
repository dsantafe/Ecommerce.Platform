namespace Ecommerce.Domain.DTOs
{
    public class OrderCreateDTO
    {
        public CustomerDTO Customer { get; set; }
        public List<CartItemDTO> Items { get; set; }
    }
}
