
using Confluent.Kafka;
using KafkaPlayground.Models;
using KafkaPlayground.Worker;
using KafkaPlayground.Worker.Infrastructure.MessageBus;
using KafkaPlayground.Worker.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<HostOptions>(config => config.ServicesStartConcurrently = true);

        builder.Services.AddHostedService<KafkaPlaygroundWorker>();


        // get some config from appsettings:
        var consumerConfig = builder.Configuration.GetSection("KafkaSettings:ConsumerConfig").Get<ConsumerConfig>();
        var parallelConsumers = builder.Configuration.GetSection("KafkaSettings:ParallelConsumers").Get<int>();
        var topic = builder.Configuration.GetSection("KafkaSettings:Topics:KafkaPlaygroundPublisher:Name").Get<string>();

        builder.Services.AddSingleton<IMessageConsumer<SampleMessage>, KafkaMessageConsumer<SampleMessage>>(sp => 
            new KafkaMessageConsumer<SampleMessage>(consumerConfig, topic, parallelConsumers));

        builder.Services.AddSingleton<IKafkaPlaygroundConsumerService, KafkaPlaygroundConsumerService>();



        var app = builder.Build();
        app.Run();
    }
}

