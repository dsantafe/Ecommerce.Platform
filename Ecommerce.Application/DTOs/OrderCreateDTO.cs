namespace Ecommerce.Application.DTOs
{
    public class OrderCreateDTO
    {
        public CustomerDTO Customer { get; set; }
        public List<CartItemDTO> Items { get; set; }
    }
}
