using AutoMapper;
using Ecommerce.Domain.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Service;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommerce.Application.Service
{
    public class ProductService(IMemoryCache memoryCache,
        IMapper mapper) : IProductService
    {
        IList<Product> _products;

        private static List<Product> SeederData() => [
                new Product
                {
                    Id = 1,
                    Name = "Product 1",
                    Description = "Product 1 Description",
                    Price = 100
                },
                new Product
                {
                    Id = 2,
                    Name = "Product 2",
                    Description = "Product 2 Description",
                    Price = 200
                },
                new Product
                {
                    Id = 3,
                    Name = "Product 3",
                    Description = "Product 3 Description",
                    Price = 300
                }
            ];

        public List<ProductDTO> GetProducts()
        {
            if (!memoryCache.TryGetValue("ProductList", out _products))
            {
                _products = SeederData();

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
            ProductDTO product = GetProducts().FirstOrDefault(x => x.Id == id);
            return product;
        }
    }
}
