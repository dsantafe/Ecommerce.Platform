using Ecommerce.Domain.DTOs;
using Ecommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.OrderManagementMS.WebApi.Endpoints
{
    public class OrderDetailEndpoint
    {
        public static void RegisterApis(WebApplication app)
        {
            app.MapGet("/api/orderdetails", ([FromServices] IOrderDetailService orderDetailsService) =>
            {
                List<OrderDetailDto> orderDetails = orderDetailsService.GetOrderDetails();
                ResponseDTO response = new()
                {
                    IsSuccess = true,
                    Message = "orders found",
                    Data = orderDetails
                };
                return Results.Ok(response);
            }).WithName("Order Detail")
            .Produces<List<OrderDto>>(200);

            app.MapGet("/api/orderdetails/{id:int}", ([FromServices] IOrderDetailService orderDetailsService,
                [FromRoute] int id) =>
            {
                ResponseDTO response = new();
                OrderDetailDto orderDetail = orderDetailsService.GetOrderDetailById(id);
                if (orderDetail is null)
                {
                    response.IsSuccess = false;
                    response.Message = "orderDetail not found";
                    response.Data = null;
                    return Results.NotFound(response);
                }

                response.IsSuccess = true;
                response.Message = "orderDetails found";
                response.Data = orderDetail;

                return Results.Ok(response);
            }).WithName("Order Detail By Id")
            .Produces<List<OrderDetailDto>>(200)
            .Produces(404);
        }
    }
}
