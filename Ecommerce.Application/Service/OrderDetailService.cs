using AutoMapper;
using Ecommerce.Domain.Data;
using Ecommerce.Domain.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Service
{
    public class OrderDetailService(IMemoryCache memoryCache,
        IMapper mapper,EcommerceContext ecommerceContext) : IOrderDetailService
    {
        IList<OrderDetail> _orderDetails;
      
        

        public List<OrderDetailDto> GetOrderDetailById(int orderId)
        {

            IUnitOfWork _unitOfWork = new UnitOfWork(ecommerceContext);
            _orderDetails = _unitOfWork.Repository<OrderDetail>().Get(filter: x=> x.OrderID == orderId, includeProperties: "Order,Product").ToList();

            var orderdetails = _orderDetails.Select(x => new OrderDetailDto()
            {
                OrderId = x.OrderID,
                ProductId = x.ProductID,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity,
                Subtotal = x.Subtotal,
                OrderName = x.Order.CustomerName,
                ProductName = x.Product.Name
            }).ToList();
            return orderdetails;
        }

    }
    
}
