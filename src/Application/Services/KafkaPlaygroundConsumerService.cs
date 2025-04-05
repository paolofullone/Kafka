using Domain.Models;
using Infrastructure.Repositories;

namespace Application.Services
{
    public class KafkaPlaygroundConsumerService(IDbSQLRepository sqlRepository) : IKafkaPlaygroundConsumerService
    {
        public async Task<bool> DoSomethingWithMessage(SampleMessage message, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{message.MessageId.ToString()}, {message.MessageDate.ToString()}");

            var checkInsertion = await sqlRepository.AddAsync(message, cancellationToken);

            return checkInsertion > 0;
        }
    }
}
