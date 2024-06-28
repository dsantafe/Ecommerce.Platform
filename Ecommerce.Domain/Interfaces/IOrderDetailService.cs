using Ecommerce.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces
{
    public interface IOrderDetailService
    {
        List<OrderDetailDto> GetOrderDetails();
        OrderDetailDto GetOrderDetailById(int orderId);
    }
}
