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
        IMapper mapper) : IOrderDetailService
    {
        IList<OrderDetail> _orderDetails;
       // protected EcommerceContext _context;
        private static List<OrderDetail> SeederData() => [
                new OrderDetail
                {
                    OrderDetailID = 1,
                    OrderID = 1,
                    ProductID = 1,
                    UnitPrice = 100,
                    Quantity = 1,
                    Subtotal = 30

                },
                 new OrderDetail
                {
                    OrderDetailID = 2,
                    OrderID = 2,
                    ProductID = 2,
                    UnitPrice = 100,
                    Quantity = 1,
                    Subtotal = 30
                },
                 new OrderDetail
                {
                    OrderDetailID = 3,
                    OrderID = 3,
                    ProductID = 3,
                    UnitPrice = 100,
                    Quantity = 2,
                    Subtotal = 30
                }
            ];

        public OrderDetailDto GetOrderDetailById(int orderId)
        {
           var orderdetail = GetOrderDetails().FirstOrDefault(x => x.OrderId == orderId); 
            return mapper.Map<OrderDetailDto>(orderdetail);
        }

        public List<OrderDetailDto> GetOrderDetails()
        {
            if (!memoryCache.TryGetValue("Orderdetail", out _orderDetails))
            {
                //_orderDetails = _context.OrderDetails.Select(x => new OrderDetail()
                //{
                //   OrderId = x.OrderId,
                //   ProductId = x.ProductId,
                //   UnitPrice = x.UnitPrice,
                //   Quantity = x.Quantity,
                //   Subtotal = x.Subtotal,
                //   Order = x.Order,
                //   Product = x.Product,
                //}).Include(x => x.Order).Include(x => x.Product).ToList();
                _orderDetails = SeederData();

                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetSlidingExpiration(TimeSpan.FromSeconds(900)) //15min
                   .SetAbsoluteExpiration(TimeSpan.FromSeconds(1800)) // 30min
                   .SetPriority(CacheItemPriority.Normal)
                   .SetSize(_orderDetails.Count);

                memoryCache.Set("Orderdetail", _orderDetails, cacheEntryOptions);
            }
             var orderdetails = _orderDetails.Select(x => new OrderDetailDto()
             {
                  OrderId = x.OrderID,
                  ProductId= x.ProductID,
                  UnitPrice = x.UnitPrice,
                  Quantity = x.Quantity,
                  Subtotal = x.Subtotal,
                  OrderName = x.Order.CustomerName,
                  ProductName = x.Product.Name
             }).ToList();
             var orderdetailDto = mapper.Map<List<OrderDetailDto>>(orderdetails);
            return orderdetailDto;
        }
    }
    
}
