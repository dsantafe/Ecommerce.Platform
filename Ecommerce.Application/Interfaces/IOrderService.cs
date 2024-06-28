namespace Ecommerce.Application.Interfaces
{
    using Ecommerce.Application.DTOs;

    public interface IOrderService
    {
        List<OrderDTO> GetOrders();
        OrderDTO GetOrderById(int id);
        OrderDTO CreateOrder(string customerName, string customerEmail, decimal total);
    }
}
