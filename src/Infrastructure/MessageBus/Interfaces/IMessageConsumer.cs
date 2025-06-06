﻿namespace Infrastructure.MessageBus.Interfaces
{
    public interface IMessageConsumer<T>
    {
        public string Topic { get; }
        Task StartConsumer(CancellationToken cancellationToken);
        Task ConsumeAsync(Func<T, CancellationToken, Task> handle, CancellationToken cancellationToken);
    }
}
