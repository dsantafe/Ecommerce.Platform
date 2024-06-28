﻿using Ecommerce.Domain.DTOs;
using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces
{
    public interface IOrderService
    {
        List<OrderDto> GetOrders();
        OrderDto GetOrderById(int id);
    }
}
