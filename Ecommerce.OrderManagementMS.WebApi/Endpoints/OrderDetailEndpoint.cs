﻿namespace Ecommerce.OrderManagementMS.WebApi.Endpoints
{
    using Ecommerce.Application.DTOs;
    using Ecommerce.Application.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Order Detail Endpoint
    /// </summary>
    public class OrderDetailEndpoint
    {
        /// <summary>
        /// Register Order Detail APIs
        /// </summary>
        /// <param name="app"></param>
        public static void RegisterApis(WebApplication app)
        {

            app.MapGet("/api/orderdetails/orderid/{id:int}", ([FromServices] IOrderDetailService orderDetailsService,
                [FromRoute] int id) =>
            {
                ResponseDTO response = new() { IsSuccess = false, Data = null };
                List<OrderDetailDTO> orderDetail = orderDetailsService.GetOrderDetailsByOrderId(id);
                if (orderDetail is null)
                {
                    response.Message = "Order Details not found";
                    return Results.NotFound(response);
                }

                response.IsSuccess = true;
                response.Message = "Order Details found";
                response.Data = orderDetail;

                return Results.Ok(response);
            }).WithName("Order Details By Order Id")
            .Produces<List<OrderDetailDTO>>(200)
            .Produces(404);
        }
    }
}
