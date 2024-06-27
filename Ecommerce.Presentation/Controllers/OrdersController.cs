namespace Ecommerce.Presentation.Controllers
{
    using Ecommerce.Application.Service;
    using Ecommerce.Domain.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    public class OrdersController : Controller
    {
        private static IList<ProductDTO> Products;
        private static List<CartItemDTO> CartItems = [];
        string urlBaseProductCatalogMs = Environment.GetEnvironmentVariable("PRODUCTS_SERVICE");
        string urlBaseOrderManagementMs = Environment.GetEnvironmentVariable("ORDERS_SERVICE");

        public IActionResult Cart()
        {
            
            ResponseDTO response = JsonConvert.DeserializeObject<ResponseDTO>(ConsumeApiService.ConsumeGet($"{urlBaseProductCatalogMs}/api/products"));
            Products = JsonConvert.DeserializeObject<IList<ProductDTO>>(response.Data.ToString());
            ViewData["Products"] = Products;
            ViewBag.CartItems = CartItems;

            string customerJson = HttpContext.Session.GetString("Customer");
            if (customerJson != null)
            {
                CustomerDTO customer = JsonConvert.DeserializeObject<CustomerDTO>(customerJson);
                return View(customer);
            }

            return View(new CustomerDTO());
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity, CustomerDTO customer)
        {
            if (quantity == 0)
                ModelState.AddModelError("quantity", "La cantidad debe ser mayor que cero.");

            if (!ModelState.IsValid)
            {
                ViewData["Products"] = Products;
                ViewBag.CartItems = CartItems;
                return View(nameof(Cart), customer);
            }

            HttpContext.Session.SetString("Customer", JsonConvert.SerializeObject(customer));

            ProductDTO product = Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                CartItemDTO existingItem = CartItems.FirstOrDefault(c => c.ProductId == productId);
                if (existingItem == null)
                {
                    CartItems.Add(new CartItemDTO
                    {
                        ProductId = product.ProductID,
                        ProductName = product.Name,
                        Quantity = quantity,
                        Price = product.Price
                    });
                }
                else
                {
                    existingItem.Quantity += quantity;
                }
            }
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        public IActionResult Checkout(CustomerDTO customer)
        {
            OrderCreateDTO order = new()
            {
                Customer = customer,
                Items = CartItems
            };

            // Aquí podrías procesar la orden, guardarla en una base de datos, etc.
            //string result = ConsumeApiService.ConsumePost($"{urlBaseOrderManagementMs}/api/orders", JsonConvert.SerializeObject(order));

            TempData["Order"] = JsonConvert.SerializeObject(order);

            return RedirectToAction(nameof(Confirmation));
        }

        public IActionResult Confirmation()
        {
            string orderJson = TempData["Order"] as string;
            if (orderJson == null)
                return RedirectToAction(nameof(Cart));

            OrderCreateDTO order = JsonConvert.DeserializeObject<OrderCreateDTO>(orderJson);

            HttpContext.Session.Remove("Customer");
            CartItems.Clear();

            return View(order);
        }
    }
}
