namespace Ecommerce.Presentation.Controllers
{
    using Ecommerce.Application.DTOs;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            if(User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                if (model.Username == "admin" && model.Password == "password")
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Name, model.Username)], CookieAuthenticationDefaults.AuthenticationScheme)),
                        new AuthenticationProperties { AllowRefresh = true });
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
