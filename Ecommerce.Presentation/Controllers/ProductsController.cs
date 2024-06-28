namespace Ecommerce.Presentation.Controllers
{
    using Ecommerce.Application.Service;
    using Ecommerce.Domain.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    public class ProductsController : Controller
    {
        IList<ProductDTO> Products;
        public IActionResult Index()
        {
            string urlBaseProductCatalogMs = Environment.GetEnvironmentVariable("PRODUCTS_SERVICE");
            ResponseDTO response = JsonConvert.DeserializeObject<ResponseDTO>(ConsumeApiService.ConsumeGet($"{urlBaseProductCatalogMs}/api/products"));
            Products = JsonConvert.DeserializeObject<IList<ProductDTO>>(response.Data.ToString());
            return View(Products);
        }
    }
}
