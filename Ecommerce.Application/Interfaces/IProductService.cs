namespace Ecommerce.Domain.Service
{
    using Ecommerce.Application.DTOs;

    public interface IProductService
    {
        List<ProductDTO> GetProducts();
        ProductDTO GetProductById(int id);
    }
}
