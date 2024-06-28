using Ecommerce.Domain.DTOs;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.OrderManagementMS.WebApi.Endpoints
{
    public class OrderEndpoint
    {
        public static void RegisterApis(WebApplication app)
        {
            app.MapGet("/api/orders", ([FromServices] IOrderService orderService) =>
            {
                List<OrderDto> orders = orderService.GetOrders();
                ResponseDTO response = new()
                {
                    IsSuccess = true,
                    Message = "orders found",
                    Data = orders
                };
                return Results.Ok(response);
            }).WithName("orders")
            .Produces<List<OrderDto>>(200);

            app.MapGet("/api/order/{id:int}",([FromServices] IOrderService orderService,
                [FromRoute] int id) =>       {
                                                ResponseDTO response = new();
                                                OrderDto order = orderService.GetOrderById(id);
                                                 if (order is null)
                                                 {
                                                     response.IsSuccess = false;
                                                     response.Message = "order not found";
                                                     response.Data = null;
                                                       return Results.NotFound(response);
                                                 }

                                                    response.IsSuccess = true;
                                                    response.Message = "order found";
                                                    response.Data = order;

                                                    return Results.Ok(response);
                                             }).WithName("order By Id")
                                               .Produces<List<OrderDto>>(200)
                                               .Produces(404);
        }
    }
}
