namespace Ecommerce.Presentation.Controllers
{
    using Ecommerce.Application.DTOs;
    using Ecommerce.Application.Service;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Authorize]
    public class OrdersController : Controller
    {
        private static IList<ProductDTO> Products;
        private static readonly List<CartItemDTO> CartItems = [];
        private readonly string urlBaseProductCatalogMs = Environment.GetEnvironmentVariable("PRODUCTS_SERVICE");
        private readonly string urlBaseOrderManagementMs = Environment.GetEnvironmentVariable("ORDERS_SERVICE");

        public ActionResult Index(int? id)
        {
            ResponseDTO response = JsonConvert.DeserializeObject<ResponseDTO>(ConsumeApiService.ConsumeGet($"{urlBaseOrderManagementMs}/api/orders"));
            IList<OrderDTO> Orders = JsonConvert.DeserializeObject<IList<OrderDTO>>(response.Data.ToString());
            if (id != null)
            {
                ResponseDTO responseDetails = JsonConvert.DeserializeObject<ResponseDTO>(ConsumeApiService.ConsumeGet($"{urlBaseOrderManagementMs}/api/orderdetails/orderid/{id}"));
                IList<OrderDetailDTO> orderDetail = JsonConvert.DeserializeObject<IList<OrderDetailDTO>>(responseDetails.Data.ToString());
                ViewData["OrderDetails"] = orderDetail;
            }

            return View(Orders);
        }

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
            if (CartItems.Count == 0)
                ModelState.AddModelError("quantity", "No se han agregado productos a la compra.");

            if (!ModelState.IsValid)
            {
                ViewData["Products"] = Products;
                ViewBag.CartItems = CartItems;
                return View(nameof(Cart), customer);
            }

            OrderCreateDTO order = new()
            {
                Customer = customer,
                Items = CartItems
            };

            string result = ConsumeApiService.ConsumePost($"{urlBaseOrderManagementMs}/api/orders", order);
            ResponseDTO response = JsonConvert.DeserializeObject<ResponseDTO>(result);
            if (!response.IsSuccess)
            {
                ModelState.AddModelError("error", response.Message);
                return RedirectToAction(nameof(Cart));
            }

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
