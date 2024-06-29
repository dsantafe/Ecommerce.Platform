namespace Ecommerce.Presentation.Bootstrapper
{
    /// <summary>
    /// Request Pipeline Builder
    /// </summary>
    public static class RequestPipelineBuilder
    {
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app"></param>
        public static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            // Añadir el middleware de sesión
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");
        }
    }
}
