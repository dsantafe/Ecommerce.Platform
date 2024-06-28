namespace Ecommerce.Application.Interfaces
{
    using Ecommerce.Application.DTOs;

    public interface IOrderDetailService
    {
        List<OrderDetailDTO> GetOrderDetailsByOrderId(int orderId);
        OrderDetailDTO GetOrderDetailById(int id);
        OrderDetailDTO CreateOrderDetail(int orderID, int productID, int quantity, decimal unitPrice);
    }
}
