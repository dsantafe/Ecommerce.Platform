using Ecommerce.Application.Service;
using Ecommerce.Domain.DTOs;
using Ecommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.OrderManagementMS.WebApi.Endpoints
{
    public class OrderDetailEndpoint
    {
        public static void RegisterApis(WebApplication app)
        {

            app.MapGet("/api/orderDetailById/{id:int}", ([FromServices] IOrderDetailService orderDetailsService,
                [FromRoute] int id) =>
            {
                ResponseDTO response = new();
               List<OrderDetailDto> orderDetail = orderDetailsService.GetOrderDetailById(id);
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
            }).WithName("order Detail By Id")
            .Produces<List<OrderDetailDto>>(200)
            .Produces(404);
        }
    }
}
