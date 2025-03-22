using Domain.Models;

namespace WebApi.Services
{
    public interface IKafkaProducerService
    {
        public Task PublishMessageAsync(KafkaMessageRequest request, CancellationToken cancellationToken);
    }
}
