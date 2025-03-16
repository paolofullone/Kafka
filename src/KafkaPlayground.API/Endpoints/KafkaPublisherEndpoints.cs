using Asp.Versioning;
using KafkaPlayground.Models;
using KafkaPlayground.Services;
using Microsoft.AspNetCore.Mvc;

namespace KafkaPlayground.Endpoints
{
    public static class KafkaPublisherEndpoints
    {
        public static IEndpointRouteBuilder MapKafkaPublisherEndpoints(this IEndpointRouteBuilder app)
        {
            // Create an API version set for grouping endpoints
            var apiVersionSet = app.NewApiVersionSet("KafkaPublisher")
                .HasApiVersion(new ApiVersion(1, 0))
                .Build();

            // Define a versioned endpoint group
            var endpointGroup = app
                .MapGroup("/api/v{version:apiVersion}/kafka/")
                .WithApiVersionSet(apiVersionSet)
                .HasApiVersion(new ApiVersion(1, 0))
                .WithOpenApi();

            // Map the POST endpoint
            endpointGroup.MapPost("", PublishKafka)
                .WithName("PublishMessages")
                .MapToApiVersion(new ApiVersion(1, 0));

            return app;
        }

        private static async Task<IResult> PublishKafka(
            [FromServices] IKafkaProducerService kafkaProducerService, 
            KafkaMessageRequest request, 
            CancellationToken cancellationToken)
        {
            await kafkaProducerService.PublishMessageAsync(request, CancellationToken.None);

            return Results.Ok(new
            {
                Message = $"Successfully processed {request.MessageAmount} messages."
            });
        }
    }
}