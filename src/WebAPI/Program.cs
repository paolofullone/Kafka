using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Confluent.Kafka;
using Infrastructure.MessageBus;
using Infrastructure.MessageBus.Interfaces;
using WebApi.Endpoints;
using WebApi.Services;
using WebApi.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0); // Default version
    options.AssumeDefaultVersionWhenUnspecified = true; // Assume default version if not specified
    options.ReportApiVersions = true; // Report available versions in the response headers
})
.AddMvc() // Add MVC support for API versioning
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // Format for grouping versions (e.g., v1, v2)
    options.SubstituteApiVersionInUrl = true; // Substitute version in URL
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    // Configure Swagger to generate multiple documents (one per API version)
    var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = $"Kafka Publisher API {description.ApiVersion}",
            Version = description.ApiVersion.ToString(),
            Description = "An API for publishing messages to Kafka."
        });
    }
});



var topicSettings = builder.Configuration.GetSection("KafkaSettings:Topics");
builder.Services.Configure<TopicSettings>(topicSettings);

var producerSettings = builder.Configuration.GetSection("KafkaSettings:ProducerConfig");
var producerConfig = producerSettings.Get<ProducerConfig>()!;
builder.Services.AddSingleton<IMessagePublisher>(_ =>
    new KafkaMessagePublisher(producerConfig)
);

builder.Services.AddScoped<IKafkaProducerService, KafkaProducerService>();

var app = builder.Build();

// Configure Swagger (optional)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    // Add a Swagger endpoint for each API version
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Kafka Publisher API {description.ApiVersion}");
    }
});


app.UseHttpsRedirection();

// Map Kafka Publisher Endpoints
app.MapKafkaPublisherEndpoints();

app.Run();