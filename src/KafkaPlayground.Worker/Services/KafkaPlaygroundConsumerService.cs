using KafkaPlayground.Models;

namespace KafkaPlayground.Worker.Services
{
    class KafkaPlaygroundConsumerService : IKafkaPlaygroundConsumerService
    {
        public Task DoSomethingWithMessage(SampleMessage message, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{message.Id.ToString()}, {message.DateTime.ToString()}");

            return Task.CompletedTask;
        }
    }
}
