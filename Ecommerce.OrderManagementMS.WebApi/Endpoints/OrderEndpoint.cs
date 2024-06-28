namespace Ecommerce.OrderManagementMS.WebApi.Endpoints
{
    using Ecommerce.Application.DTOs;
    using Ecommerce.Application.Interfaces;
    using FluentValidation;
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Order Endpoint
    /// </summary>
    public class OrderEndpoint
    {
        /// <summary>
        /// Register Order APIs
        /// </summary>
        /// <param name="app"></param>
        public static void RegisterApis(WebApplication app)
        {
            app.MapGet("/api/orders", ([FromServices] IOrderService orderService) =>
            {
                List<OrderDTO> orders = orderService.GetOrders();
                ResponseDTO response = new()
                {
                    IsSuccess = true,
                    Message = "Orders found",
                    Data = orders
                };
                return Results.Ok(response);
            }).WithName("Orders")
            .WithDescription("Obtiene una lista de órdenes (OrderDTO). " +
                     "Recupera todas las órdenes del repositorio, las mapea a objetos OrderDTO y las retorna.")
            .WithOpenApi()
            .WithTags("Orders")
            .Produces<ResponseDTO>(200);

            app.MapGet("/api/orders/{id:int}", ([FromServices] IOrderService orderService,
                [FromRoute] int id) =>
            {
                ResponseDTO response = new() { IsSuccess = false, Data = null };
                OrderDTO order = orderService.GetOrderById(id);
                if (order is null)
                {
                    response.Message = "Order not found";
                    return Results.NotFound(response);
                }

                response.IsSuccess = true;
                response.Message = "Order found";
                response.Data = order;

                return Results.Ok(response);
            }).WithName("Order By Id")
            .WithDescription("Obtiene una orden específica (OrderDTO) por su identificador. " +
                     "Recupera la orden del repositorio utilizando el identificador proporcionado " +
                     "y la mapea a un objeto OrderDTO.")
            .WithOpenApi(generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "El identificador de la orden.";
                return generatedOperation;
            })
            .WithTags("Orders")
            .Produces<ResponseDTO>(200)
            .Produces(404);

            app.MapPost("/api/orders", async ([FromServices] IValidator<OrderCreateDTO> validator,
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

                    OrderDTO order = orderService.CreateOrder(customerName, customerEmail, total);
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
            }).WithName("Create Order")
            .WithDescription("Crea una nueva orden (OrderDTO). " +
                     "Inicializa una nueva orden con los valores proporcionados, la inserta en el repositorio " +
                     "y la guarda en la base de datos. " +
                     "Finalmente, mapea la orden creada a un objeto OrderDTO y la retorna.")
            .WithOpenApi(generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "El objeto de la orden con su detalle.";
                return generatedOperation;
            })
            .WithTags("Orders")
            .Produces(400)
            .Produces(201)
            .Produces<ResponseDTO>(200);
        }
    }
}
