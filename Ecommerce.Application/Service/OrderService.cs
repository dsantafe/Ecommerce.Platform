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

        /// <summary>
        /// Obtiene una lista de órdenes (OrderDTO).
        /// Recupera todas las órdenes del repositorio, las mapea a objetos OrderDTO y las retorna.
        /// </summary>
        /// <returns>Lista de órdenes mapeadas como OrderDTO.</returns>
        public List<OrderDTO> GetOrders()
        {
            IList<Order> orders = unitOfWork.Repository<Order>().Get().ToList();
            return orders.Select(x => mapper.Map<OrderDTO>(x)).ToList();
        }

        /// <summary>
        /// Obtiene una orden específica (OrderDTO) por su identificador.
        /// Recupera la orden del repositorio utilizando el identificador proporcionado y la mapea a un objeto OrderDTO.
        /// </summary>
        /// <param name="id">El identificador de la orden.</param>
        /// <returns>La orden mapeada como OrderDTO.</returns>
        public OrderDTO GetOrderById(int id)
        {
            OrderDTO order = mapper.Map<OrderDTO>(unitOfWork.Repository<Order>().GetByID(id));
            return order;
        }

        /// <summary>
        /// Crea una nueva orden (OrderDTO).
        /// Inicializa una nueva orden con los valores proporcionados, la inserta en el repositorio
        /// y la guarda en la base de datos.
        /// Finalmente, mapea la orden creada a un objeto OrderDTO y la retorna.
        /// </summary>
        /// <param name="customerName">Nombre del cliente asociado a la orden.</param>
        /// <param name="customerEmail">Correo electrónico del cliente asociado a la orden.</param>
        /// <param name="total">Total de la orden.</param>
        /// <returns>La orden creada mapeada como OrderDTO.</returns>
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
            return mapper.Map<OrderDTO>(order);
        }
    }
}
