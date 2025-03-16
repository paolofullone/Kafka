using KafkaPlayground.Models;
using KafkaPlayground.Worker.Infrastructure.MessageBus;
using KafkaPlayground.Worker.Services;
using Microsoft.Extensions.Hosting;

namespace KafkaPlayground.Worker
{
    class KafkaPlaygroundWorker(
        IMessageConsumer<SampleMessage> consumer,
        IKafkaPlaygroundConsumerService service) : BackgroundService
    {

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("starting worker...");

            await consumer.StartConsumer(cancellationToken);

            await Task.FromResult(base.StartAsync(cancellationToken));
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await consumer.ConsumeAsync(service.DoSomethingWithMessage, stoppingToken);

            Console.WriteLine("Stoping workers...");
        }
    }
}
