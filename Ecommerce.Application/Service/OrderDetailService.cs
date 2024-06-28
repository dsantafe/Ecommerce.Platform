namespace Ecommerce.Application.Service
{
    using AutoMapper;
    using Ecommerce.Application.DTOs;
    using Ecommerce.Application.Interfaces;
    using Ecommerce.Domain.Data;
    using Ecommerce.Domain.Entities;

    public class OrderDetailService(IMapper mapper, EcommerceContext ecommerceContext) : IOrderDetailService
    {
        private readonly UnitOfWork unitOfWork = new(ecommerceContext);

        public List<OrderDetailDTO> GetOrderDetailsByOrderId(int orderId)
        {
            List<OrderDetail> orderDetails = unitOfWork.Repository<OrderDetail>()
                .Get(filter: x => x.OrderID == orderId, includeProperties: "Order,Product")
                .ToList();

            return orderDetails.Select(x => new OrderDetailDTO
            {
                OrderDetailID = x.OrderDetailID,
                OrderId = x.OrderID,
                ProductId = x.ProductID,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity,
                Subtotal = x.Subtotal,
                ProductName = x.Product.Name
            }).ToList();
        }

        public OrderDetailDTO GetOrderDetailById(int id)
        {
            OrderDetailDTO orderDetail = mapper.Map<OrderDetailDTO>(unitOfWork.Repository<OrderDetail>().GetByID(id));
            return orderDetail;
        }

        public OrderDetailDTO CreateOrderDetail(int orderID, int productID, int quantity, decimal unitPrice)
        {
            OrderDetail orderDetail = new()
            {
                OrderID = orderID,
                ProductID = productID,
                Quantity = quantity,
                UnitPrice = unitPrice
            };
            unitOfWork.Repository<OrderDetail>().Insert(orderDetail);
            unitOfWork.Save();
            return mapper.Map<OrderDetailDTO>(orderDetail);
        }
    }
}
