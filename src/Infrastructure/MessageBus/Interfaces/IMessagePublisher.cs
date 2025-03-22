namespace Infrastructure.MessageBus.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(T message, string topic, CancellationToken cancellationToken = default);
    }
}
