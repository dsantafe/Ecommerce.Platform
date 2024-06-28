using Ecommerce.Domain.DTOs;
using Ecommerce.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
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

            app.MapGet("/api/order/{id:int}", ([FromServices] IOrderService orderService,
                [FromRoute] int id) =>
            {
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

            app.MapPost("/api/order", async ([FromServices] IValidator<OrderCreateDTO> validator,
                [FromServices] IOrderService orderService,
                [FromServices] IOrderDetailService orderDetailService,
                [FromBody] OrderCreateDTO orderCreateDTO) =>
            {
                ResponseDTO response = new() { IsSuccess = false, Data = null };
                ValidationResult validationResult = await validator.ValidateAsync(orderCreateDTO);
                if (!validationResult.IsValid)
                {
                    response.Message = string.Join(", ", validationResult.Errors.Select(failure => $"Error: {failure.ErrorMessage}"));
                    return Results.Ok(response);
                }

                try
                {
                    string customerName = orderCreateDTO.Customer.Name;
                    string customerEmail = orderCreateDTO.Customer.Email;
                    decimal total = orderCreateDTO.Items.Sum(i => i.Subtotal);

                    OrderDto order = orderService.CreateOrder(customerName, customerEmail, total);
                    if (order is null)
                    {
                        response.Message = "Order Not Created";
                        return Results.BadRequest(response);
                    }

                    orderCreateDTO.Items.ForEach(n =>
                    {
                        orderDetailService.CreateOrderDetail(order.OrderID, n.ProductId, n.Quantity, n.Subtotal);
                    });

                    response.IsSuccess = true;
                    response.Message = "Order Created";
                    response.Data = order;

                    return Results.Created($"/api/order/{order.OrderID}", response);
                }
                catch (Exception ex)
                {
                    response.Message = ex.Message;
                    return Results.Ok(response);
                }
            }).WithName("Create Order");
        }
    }
}
