using Domain.Models;
using Infrastructure.Repositories;

namespace Application.Services
{
    public class KafkaPlaygroundConsumerService(ISQLRepository repository) : IKafkaPlaygroundConsumerService
    {
        public async Task<bool> DoSomethingWithMessage(SampleMessage message, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{message.MessageId.ToString()}, {message.MessageDate.ToString()}");

            return await repository.AddAsync(message, cancellationToken) > 0;
        }
    }
}
