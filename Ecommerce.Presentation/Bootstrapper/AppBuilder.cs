namespace Ecommerce.Presentation.Bootstrapper
{
    using Microsoft.AspNetCore.Authentication.Cookies;

    /// <summary>
    /// App Builder 
    /// </summary>
    public static class AppBuilder
    {
        /// <summary>
        /// Get App
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static WebApplication GetApp(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Configuración de la sesión
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Duración de la sesión
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; // Necesario para la GDPR si aplica
            });
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                });

            WebApplication app = builder.Build();
            return app;
        }
    }
}
