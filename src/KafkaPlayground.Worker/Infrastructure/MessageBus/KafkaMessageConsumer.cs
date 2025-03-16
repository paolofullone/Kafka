
using Confluent.Kafka;
using System.Text.Json;

namespace KafkaPlayground.Worker.Infrastructure.MessageBus
{
    class KafkaMessageConsumer<T> : IMessageConsumer<T>
    {
        private readonly List<IConsumer<Null, byte[]>> _consumers;
        private readonly string _topic;
        private readonly JsonSerializerOptions _options = new(JsonSerializerDefaults.Web);

        public KafkaMessageConsumer(ConsumerConfig consumerConfig, string topic, int parallelConsumers)
        {
            _consumers = new List<IConsumer<Null, byte[]>>();
            for (int i = 1; i <= parallelConsumers; i++)
            {
                _consumers.Add(new ConsumerBuilder<Null, byte[]>(consumerConfig).Build());
                Console.WriteLine($"Criando consumer {i}");
            }
            _topic = topic;
        }

        public string Topic => _topic;

        public Task StartConsumer(CancellationToken cancellationToken)
        {
            foreach (var consumer in _consumers)
            {
                consumer.Subscribe(_topic);
            }
            return Task.CompletedTask;
        }

        public async Task ConsumeAsync(Func<T, CancellationToken, Task> handle, CancellationToken cancellationToken)
        {
            var tasks = _consumers.Select(consumer => Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var message = consumer.Consume(cancellationToken);
                        var consumeResult = await JsonSerializer.DeserializeAsync<T>(new MemoryStream(message.Message.Value), _options, cancellationToken);

                        if (cancellationToken.IsCancellationRequested)
                            break;

                        await handle(consumeResult!, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }));
            await Task.WhenAll(tasks.ToArray());
        }
    }
}
