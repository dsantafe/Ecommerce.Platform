namespace Ecommerce.Presentation.Controllers
{
    using Ecommerce.Application.DTOs;
    using Ecommerce.Application.Service;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    public class ProductsController : Controller
    {
        private readonly string urlBaseProductCatalogMs = Environment.GetEnvironmentVariable("PRODUCTS_SERVICE");

        public IActionResult Index()
        {
            ResponseDTO response = JsonConvert.DeserializeObject<ResponseDTO>(ConsumeApiService.ConsumeGet($"{urlBaseProductCatalogMs}/api/products"));
            IList<ProductDTO> Products = JsonConvert.DeserializeObject<IList<ProductDTO>>(response.Data.ToString());
            return View(Products);
        }
    }
}
