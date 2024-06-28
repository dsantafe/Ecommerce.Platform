using Ecommerce.Domain.DTOs;

namespace Ecommerce.Domain.Interfaces
{
    public interface IOrderDetailService
    {
        List<OrderDetailDto> GetOrderDetails();
        OrderDetailDto GetOrderDetailById(int orderId);
        OrderDetailDto CreateOrderDetail(int orderID, int productID, int quantity, decimal unitPrice);
    }
}
