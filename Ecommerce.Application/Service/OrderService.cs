using AutoMapper;
using Ecommerce.Domain.Data;
using Ecommerce.Domain.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommerce.Application.Service
{
    public class OrderService(IMemoryCache memoryCache,
        IMapper mapper, EcommerceContext ecommerceContext) : IOrderService
    {
        
        IList<Order> _order;
        public OrderDto GetOrderById(int id)
        {
            OrderDto order = GetOrders().FirstOrDefault(x => x.OrderID == id);
            return order;
        }

        public List<OrderDto> GetOrders()
        {
            if (!memoryCache.TryGetValue("OrderList", out _order))
            {
               IUnitOfWork unitOfWork = new UnitOfWork(ecommerceContext);
                _order = unitOfWork.Repository<Order>().Get().ToList();


                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetSlidingExpiration(TimeSpan.FromSeconds(900)) //15min
                   .SetAbsoluteExpiration(TimeSpan.FromSeconds(1800)) // 30min
                   .SetPriority(CacheItemPriority.Normal)
                   .SetSize(_order.Count);

                memoryCache.Set("OrderList", _order, cacheEntryOptions);
            }

            return _order.Select(x => mapper.Map<OrderDto>(x)).ToList();
        }
    }
}
