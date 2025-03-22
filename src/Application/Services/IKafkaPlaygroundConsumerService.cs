﻿using Domain.Models;

namespace Application.Services
{
    public interface IKafkaPlaygroundConsumerService
    {
        Task<bool> DoSomethingWithMessage(SampleMessage message, CancellationToken cancellationToken);
    }
}
