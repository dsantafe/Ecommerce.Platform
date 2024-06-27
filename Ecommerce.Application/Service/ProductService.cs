namespace Ecommerce.Application.Service
{
    using AutoMapper;
    using Ecommerce.Domain.Data;
    using Ecommerce.Domain.DTOs;
    using Ecommerce.Domain.Entities;
    using Ecommerce.Domain.Interfaces;
    using Ecommerce.Domain.Service;
    using Microsoft.Extensions.Caching.Memory;

    public class ProductService(IMemoryCache memoryCache,
        IMapper mapper,
        EcommerceContext ecommerceContext) : IProductService
    {
        IList<Product> _products;

        public List<ProductDTO> GetProducts()
        {
            if (!memoryCache.TryGetValue("ProductList", out _products))
            {
                IUnitOfWork _unitOfWork = new UnitOfWork(ecommerceContext);
                _products = _unitOfWork.Repository<Product>().Get().ToList();

                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetSlidingExpiration(TimeSpan.FromSeconds(900)) //15min
                   .SetAbsoluteExpiration(TimeSpan.FromSeconds(1800)) // 30min
                   .SetPriority(CacheItemPriority.Normal)
                   .SetSize(_products.Count);

                memoryCache.Set("ProductList", _products, cacheEntryOptions);
            }

            return _products.Select(x => mapper.Map<ProductDTO>(x)).ToList();
        }

        public ProductDTO GetProductById(int id)
        {
            ProductDTO product = GetProducts().FirstOrDefault(x => x.ProductID == id);
            return product;
        }
    }
}
