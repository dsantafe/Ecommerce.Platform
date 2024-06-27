﻿namespace Ecommerce.ProductCatalogMS.WebApi.Bootstrapper
{
    using Ecommerce.Application.Service;
    using Ecommerce.Domain.Constants;
    using Ecommerce.Domain.Service;
    using FluentValidation;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.OpenApi.Models;
    using System.Reflection;

    /// <summary>
    /// App Builder 
    /// </summary>
    public static class AppBuilder
    {
        public static WebApplication GetApp(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHealthChecks();
            MemoryCache memoryCache = new(new MemoryCacheOptions { SizeLimit = CacheConstant.SizeLimit });
            builder.Services.AddSingleton<IMemoryCache>(memoryCache);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Product Catalog Service",
                    Description = "Should provide product listings.",
                    Contact = new OpenApiContact
                    {
                        Name = "Microsoft Learn",
                        Url = new Uri("https://learn.microsoft.com/")
                    }
                });

                string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            // Services
            builder.Services.AddScoped<IProductService, ProductService>();

            // Logs
            builder.Logging.ClearProviders();

            WebApplication app = builder.Build();
            return app;
        }
    }
}
