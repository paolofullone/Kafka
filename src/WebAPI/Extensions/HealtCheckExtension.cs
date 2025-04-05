using Confluent.Kafka;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace WebApi.Extensions
{
    public static class HealtCheckExtension
    {
        public static IServiceCollection AddApplicationHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {

            var kafkaConfig = configuration.GetSection("KafkaSettings").Get<ProducerConfig>();
            var kafkaTopic = configuration.GetSection("KafkaSettings:Topics:KafkaHealthCheck:Name").Get<string>();
            var sqlServer = configuration.GetConnectionString("SqlServer")!;
            var oracle = configuration.GetConnectionString("Oracle")!;

            var databaseQueries = new Dictionary<string, (string ConnectionString, IList<string> Queries, string Provider)>
            {
                { "SqlServer", (sqlServer, new List<string> { "SELECT 1 FROM dbo.SAMPLE_MESSAGES" }, "SqlServer" ) },
                { "Oracle", (oracle, new List<string> { "" +
                    "SELECT 1 FROM ADMIN.SAMPLE_MESSAGES",
                    "SELECT 1 FROM ADMIN.WO_ACCESS"
                }, "Oracle") }
            };

            services.AddSingleton(new HealthCheckExtensionMultiDatabase(databaseQueries));

            services.AddHealthChecks()
                    .AddCheck(name: "live", check: () => HealthCheckResult.Healthy(), tags: new[] { "live", "ready" }, TimeSpan.FromSeconds(30))
                    .AddKafka(kafkaConfig!, kafkaTopic!, name: "Kafka")
                    .AddSqlServer(sqlServer, name: "SqlServer", failureStatus: HealthStatus.Unhealthy, tags: new[] { "db", "mssql" })
                    .AddOracle(oracle, name: "Oracle", failureStatus: HealthStatus.Unhealthy, tags: new[] { "db", "oracle" })
                    .AddCheck<HealthCheckExtensionMultiDatabase>(name: "MultiDatabase", failureStatus: HealthStatus.Unhealthy, tags: new[] { "db", "mssql", "oracle" });

            return services;
        }

        public static void MapApplicationHealthChecks(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks("health/ready", new HealthCheckOptions
            {
                Predicate = (check) =>
                    check.Tags.Contains("ready") ||
                    check.Tags.Contains("db") ||
                    check.Tags.Contains("mssql") ||
                    check.Tags.Contains("Kafka") ||
                    check.Tags.Contains("oracle"),
                ResponseWriter = WriteCustomResponse
            });

            endpoints.MapHealthChecks("health/live", new HealthCheckOptions
            {
                Predicate = (check) =>
                    check.Tags.Contains("ready") ||
                    check.Tags.Contains("db") ||
                    check.Tags.Contains("mssql") ||
                    check.Tags.Contains("Kafka") ||
                    check.Tags.Contains("oracle"),
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
