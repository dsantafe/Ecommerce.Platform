namespace Ecommerce.ProductCatalogMS.WebApi.Endpoints
{
    using Ecommerce.Domain.DTOs;
    using Ecommerce.Domain.Service;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    /// <summary>
    /// Minimal APIs Health Checker and Metrics Collector
    /// </summary>
    public class ProductEndpoint
    {
        /// <summary>
        /// Registro de APIs
        /// </summary>
        /// <param name="app"></param>
        public static void RegisterApis(WebApplication app)
        {
            app.MapGet("/api/products", ([FromServices] IProductService productService) =>
            {
                List<ProductDTO> products = productService.GetProducts();
                ResponseDTO response = new()
                {
                    IsSuccess = true,
                    Message = "Products found",
                    Data = products
                };
                return Results.Ok(response);
            }).WithName("Products")
            .Produces<List<ProductDTO>>(200);

            app.MapGet("/api/products/{id:int}", ([FromServices] IProductService productService,
                [FromRoute] int id) =>
            {
                ResponseDTO response = new();
                ProductDTO product = productService.GetProductById(id);
                if(product is null)
                {
                    response.IsSuccess = false;
                    response.Message = "Product not found";
                    response.Data = null;
                    return Results.NotFound(response);
                }

                response.IsSuccess = true;
                response.Message = "Product found";
                response.Data = product;

                return Results.Ok(response);
            }).WithName("Product By Id")
            .Produces<List<ProductDTO>>(200)
            .Produces(404);
        }
    }
}
