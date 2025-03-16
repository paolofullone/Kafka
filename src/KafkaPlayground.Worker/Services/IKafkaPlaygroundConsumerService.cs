using KafkaPlayground.Models;

namespace KafkaPlayground.Worker.Services
{
    interface IKafkaPlaygroundConsumerService
    {
        Task DoSomethingWithMessage(SampleMessage message, CancellationToken cancellationToken);
    }
}
