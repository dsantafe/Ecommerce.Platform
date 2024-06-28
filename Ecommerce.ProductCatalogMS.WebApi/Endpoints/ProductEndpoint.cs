namespace Ecommerce.ProductCatalogMS.WebApi.Endpoints
{
    using Ecommerce.Application.DTOs;
    using Ecommerce.Domain.Service;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    /// <summary>
    /// Product Endpoint
    /// </summary>
    public class ProductEndpoint
    {
        /// <summary>
        /// Register Product APIs
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
            .WithDescription("Obtiene una lista de productos (ProductDTO). " +
                     "Primero, intenta recuperar los productos de la memoria caché utilizando la clave 'ProductList'. " +
                     "Si los productos no están en caché, los recupera del repositorio y los almacena en la caché " +
                     "con opciones específicas de expiración y prioridad. " +
                     "Finalmente, mapea y devuelve la lista de productos como objetos ProductDTO.")
            .WithOpenApi()
            .WithTags("Products")
            .Produces<ResponseDTO>(200);

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
            .WithDescription("Obtiene un producto específico (ProductDTO) por su identificador. " +
                     "Recupera el producto del repositorio utilizando el identificador proporcionado y lo mapea a un objeto ProductDTO.")
            .WithOpenApi(generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "El identificador del producto.";
                return generatedOperation;
            })
            .WithTags("Products")
            .Produces<ResponseDTO>(200)
            .Produces(404);
        }
    }
}
