namespace Ecommerce.Domain.Service
{
    using Ecommerce.Domain.DTOs;

    public interface IProductService
    {
        List<ProductDTO> GetProducts();
        ProductDTO GetProductById(int id);
    }
}
