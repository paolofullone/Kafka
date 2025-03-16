using KafkaPlayground.Models;

namespace KafkaPlayground.Services
{
    public interface IKafkaProducerService
    {
        public Task PublishMessageAsync(KafkaMessageRequest request, CancellationToken cancellationToken);
    }
}
