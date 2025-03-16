
using KafkaPlayground.Infrastructore.MessageBus;
using KafkaPlayground.Models;
using KafkaPlayground.Settings;
using Microsoft.Extensions.Options;

namespace KafkaPlayground.Services
{
    public class KafkaProducerService(IMessagePublisher publisher, IOptions<TopicSettings> options) : IKafkaProducerService
    {
        public async Task PublishMessageAsync(KafkaMessageRequest request, CancellationToken cancellationToken)
        {
            for (var i = 0; i < request.MessageAmount; i++)
            {
                var message = new SampleMessage
                {
                    Id = Guid.NewGuid(),
                    DateTime = DateTime.Now
                };

                await publisher.PublishAsync(message, options.Value.KafkaPlaygroundPublisher.Name, cancellationToken);
            }
        }
    }
}
