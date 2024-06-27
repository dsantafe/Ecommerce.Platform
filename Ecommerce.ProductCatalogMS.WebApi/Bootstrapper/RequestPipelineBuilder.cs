namespace Ecommerce.ProductCatalogMS.WebApi.Bootstrapper
{
    /// <summary>
    /// Request Pipeline Builder
    /// </summary>
    public static class RequestPipelineBuilder
    {
        public static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
        }
    }
}
