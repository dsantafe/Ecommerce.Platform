namespace Ecommerce.Application.Service
{
    using AutoMapper;
    using Ecommerce.Application.DTOs;
    using Ecommerce.Application.Interfaces;
    using Ecommerce.Domain.Data;
    using Ecommerce.Domain.Entities;

    public class OrderService(IMapper mapper, EcommerceContext ecommerceContext) : IOrderService
    {
        private readonly UnitOfWork unitOfWork = new(ecommerceContext);        

        public List<OrderDTO> GetOrders()
        {
            IList<Order> orders = unitOfWork.Repository<Order>().Get().ToList();
            return orders.Select(x => mapper.Map<OrderDTO>(x)).ToList();
        }

        public OrderDTO GetOrderById(int id)
        {
            OrderDTO order = mapper.Map<OrderDTO>(unitOfWork.Repository<Order>().GetByID(id));
            return order;
        }

        public OrderDTO CreateOrder(string customerName, string customerEmail, decimal total)
        {
            Order order = new()
            {
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                OrderDate = DateTime.UtcNow,
                Total = total
            };
            unitOfWork.Repository<Order>().Insert(order);
            unitOfWork.Save();
            return GetOrderById(order.OrderID);
        }
    }
}
