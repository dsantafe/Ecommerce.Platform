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

        /// <summary>
        /// Obtiene los detalles de una orden específica (OrderDetailDTO) por el identificador de la orden.
        /// Recupera los detalles de la orden del repositorio utilizando el identificador de la orden proporcionado
        /// y los mapea a objetos OrderDetailDTO.
        /// </summary>
        /// <param name="orderId">El identificador de la orden.</param>
        /// <returns>Lista de detalles de la orden mapeados como OrderDetailDTO.</returns>
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

        /// <summary>
        /// Obtiene un detalle de orden específico (OrderDetailDTO) por su identificador.
        /// Recupera el detalle de la orden del repositorio utilizando el identificador proporcionado y lo mapea a un objeto OrderDetailDTO.
        /// </summary>
        /// <param name="id">El identificador del detalle de la orden.</param>
        /// <returns>El detalle de la orden mapeado como OrderDetailDTO.</returns>
        public OrderDetailDTO GetOrderDetailById(int id)
        {
            OrderDetailDTO orderDetail = mapper.Map<OrderDetailDTO>(unitOfWork.Repository<OrderDetail>().GetByID(id));
            return orderDetail;
        }

        /// <summary>
        /// Crea un nuevo detalle de orden (OrderDetailDTO).
        /// Inicializa un nuevo detalle de orden con los valores proporcionados, lo inserta en el repositorio
        /// y lo guarda en la base de datos.
        /// Finalmente, mapea el detalle de orden creado a un objeto OrderDetailDTO y lo retorna.
        /// </summary>
        /// <param name="orderID">El identificador de la orden.</param>
        /// <param name="productID">El identificador del producto.</param>
        /// <param name="quantity">La cantidad del producto.</param>
        /// <param name="unitPrice">El precio unitario del producto.</param>
        /// <returns>El detalle de la orden creado mapeado como OrderDetailDTO.</returns>
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
