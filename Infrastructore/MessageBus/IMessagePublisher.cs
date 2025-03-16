namespace KafkaPlayground.Infrastructore.MessageBus
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(T message, string topic, CancellationToken cancellationToken = default);
    }
}
