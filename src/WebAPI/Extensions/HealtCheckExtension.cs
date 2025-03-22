using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace WebApi.Extensions
{
    public static class HealtCheckExtension
    {
        public static IServiceCollection AddApplicationHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHealthChecks()
                .AddCheck("live", check: () => HealthCheckResult.Healthy(), tags: new[] { "live", "ready" }, TimeSpan.FromSeconds(30))
                .AddSqlServer(
                    configuration.GetConnectionString("SqlServer")!,
                    name: "SqlServer",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { "db", "mssql" });

            return services;
        }

        public static void MapApplicationHealthChecks(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks("health/ready", new HealthCheckOptions
            {
                Predicate = (check) => check.Tags.Contains("ready") || check.Tags.Contains("db") || check.Tags.Contains("mssql"),
                ResponseWriter = WriteCustomResponse
            });

            endpoints.MapHealthChecks("health/live", new HealthCheckOptions
            {
                Predicate = (check) => check.Tags.Contains("ready") || check.Tags.Contains("db") || check.Tags.Contains("mssql"),
                ResponseWriter = WriteCustomResponse
            });

            // both equal for simplicity sake, but we can have one to check during the deploy (ready) an another to check if the app is running (live)
        }

        private static async Task WriteCustomResponse(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = report.Status.ToString(),
                results = report.Entries.Select(entry => new
                {
                    key = entry.Key,
                    value = new
                    {
                        status = entry.Value.Status.ToString(),
                        description = entry.Value.Description,
                        data = entry.Value.Data,
                        exception = entry.Value.Exception?.Message
                    }
                })
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                WriteIndented = true,
            });

            await context.Response.WriteAsync(json);
        }
    }
}
