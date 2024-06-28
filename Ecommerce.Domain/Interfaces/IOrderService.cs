using Ecommerce.Domain.DTOs;

namespace Ecommerce.Domain.Interfaces
{
    public interface IOrderService
    {
        List<OrderDto> GetOrders();
        OrderDto GetOrderById(int id);
        OrderDto CreateOrder(string customerName, string customerEmail, decimal total);
    }
}
