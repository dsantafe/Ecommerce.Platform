namespace Ecommerce.ProductCatalogMS.WebApi.Endpoints
{
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    /// <summary>
    /// Minimal APIs Health Checker and Metrics Collector
    /// </summary>
    public class CommonEndpoint
    {
        /// <summary>
        /// Registro de APIs
        /// </summary>
        /// <param name="app"></param>
        public static void RegisterApis(WebApplication app)
        {
            app.MapGet("/api/health", async (HealthCheckService healthCheckService) =>
            {
                HealthReport report = await healthCheckService.CheckHealthAsync();
                return report.Status == HealthStatus.Healthy ?
                    Results.Ok(report) : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
            })
            .WithTags(["Health Checker"])
            .Produces(200)
            .ProducesProblem(503)
            .Produces(429);
        }
    }
}
