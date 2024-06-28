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

        public ProductDTO GetProductById(int id)
        {
            ProductDTO product = mapper.Map<ProductDTO>(unitOfWork.Repository<Product>().GetByID(id));
            return product;
        }
    }
}
