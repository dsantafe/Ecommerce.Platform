using Ecommerce.Application.Service;
using Ecommerce.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ecommerce.Presentation.Controllers
{
    public class ProductsController : Controller
    {
        IList<ProductDTO> Products;
        public IActionResult Index()
        {
            ResponseDTO response = JsonConvert.DeserializeObject<ResponseDTO>(ConsumeApiService.ConsumeGet("http://localhost:5174/api/products"));
            Products = JsonConvert.DeserializeObject<IList<ProductDTO>>(response.Data.ToString());
            return View(Products);
        }
    }
}
