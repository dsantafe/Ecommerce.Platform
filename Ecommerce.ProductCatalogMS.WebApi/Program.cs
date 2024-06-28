using Ecommerce.ProductCatalogMS.WebApi.Bootstrapper;
using Ecommerce.ProductCatalogMS.WebApi.Endpoints;

// Get WebApplication instance
WebApplication app = AppBuilder.GetApp(args);

// Configure Request Pipeline
RequestPipelineBuilder.Configure(app);

// Configure APIs 
CommonEndpoint.RegisterApis(app);
ProductEndpoint.RegisterApis(app);

// Start the app
app.Run();

