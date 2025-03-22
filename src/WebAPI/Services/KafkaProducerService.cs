using Domain.Models;
using Infrastructure.MessageBus.Interfaces;
using Microsoft.Extensions.Options;
using WebApi.Settings;

namespace WebApi.Services
{
    public class KafkaProducerService(IMessagePublisher publisher, IOptions<TopicSettings> options) : IKafkaProducerService
    {
        public async Task PublishMessageAsync(KafkaMessageRequest request, CancellationToken cancellationToken)
        {
            _ = Task.Run(async () =>
            {
                for (var i = 0; i < request.MessageAmount; i++)
                {
                    var message = new SampleMessage
                    {
                        MessageId = Guid.NewGuid(),
                        MessageDate = DateTime.Now
                    };

                    await publisher.PublishAsync(message, options.Value.KafkaPlaygroundPublisher.Name, cancellationToken);
                }
            });
        }
    }
}
