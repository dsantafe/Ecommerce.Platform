using Ecommerce.OrderManagementMS.WebApi.Bootstrapper;
using Ecommerce.OrderManagementMS.WebApi.Endpoints;

// Get WebApplication instance
WebApplication app = AppBuilder.GetApp(args);

// Configure Request Pipeline
RequestPipelineBuilder.Configure(app);

// Configure APIs 
CommonEndpoint.RegisterApis(app);
OrderEndpoint.RegisterApis(app);
OrderDetailEndpoint.RegisterApis(app);

// Start the app
app.Run();
