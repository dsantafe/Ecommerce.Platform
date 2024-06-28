namespace Ecommerce.Application.Service
{
    using AutoMapper;
    using Ecommerce.Application.DTOs;
    using Ecommerce.Domain.Data;
    using Ecommerce.Domain.Entities;
    using Ecommerce.Domain.Service;
    using Microsoft.Extensions.Caching.Memory;

    public class ProductService(IMemoryCache memoryCache,
        IMapper mapper,
        EcommerceContext ecommerceContext) : IProductService
    {
        private readonly UnitOfWork unitOfWork = new(ecommerceContext);
        IList<Product> products;

        /// <summary>
        /// Obtiene una lista de productos (ProductDTO).
        /// Primero, intenta recuperar los productos de la memoria caché utilizando la clave "ProductList".
        /// Si los productos no están en caché, los recupera del repositorio y los almacena en la caché
        /// con opciones específicas de expiración y prioridad.
        /// Finalmente, mapea y devuelve la lista de productos como objetos ProductDTO.
        /// </summary>
        /// <returns>Lista de productos mapeados como ProductDTO.</returns>
        public List<ProductDTO> GetProducts()
        {
            if (!memoryCache.TryGetValue("ProductList", out products))
            {
                products = unitOfWork.Repository<Product>().Get().ToList();

                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetSlidingExpiration(TimeSpan.FromSeconds(900)) //15min
                   .SetAbsoluteExpiration(TimeSpan.FromSeconds(1800)) // 30min
                   .SetPriority(CacheItemPriority.Normal)
                   .SetSize(products.Count);

                memoryCache.Set("ProductList", products, cacheEntryOptions);
            }

            return products.Select(x => mapper.Map<ProductDTO>(x)).ToList();
        }

        /// <summary>
        /// Obtiene un producto específico (ProductDTO) por su identificador.
        /// Recupera el producto del repositorio utilizando el identificador proporcionado y lo mapea a un objeto ProductDTO.
        /// </summary>
        /// <param name="id">El identificador del producto.</param>
        /// <returns>El producto mapeado como ProductDTO.</returns>
        public ProductDTO GetProductById(int id)
        {
            ProductDTO product = mapper.Map<ProductDTO>(unitOfWork.Repository<Product>().GetByID(id));
            return product;
        }
    }
}
