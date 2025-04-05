using Application.Services;
using Confluent.Kafka;
using Domain.Models;
using Infrastructure.DbFactory;
using Infrastructure.DbFactory.Interfaces;
using Infrastructure.MessageBus;
using Infrastructure.MessageBus.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Worker;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<HostOptions>(config => config.ServicesStartConcurrently = true);

        builder.Services.AddHostedService<KafkaPlaygroundWorker>();

        var consumerConfig = builder.Configuration.GetSection("KafkaSettings:ConsumerConfig").Get<ConsumerConfig>();
        var parallelConsumers = builder.Configuration.GetSection("KafkaSettings:ParallelConsumers").Get<int>();
        var topic = builder.Configuration.GetSection("KafkaSettings:Topics:KafkaPlaygroundPublisher:Name").Get<string>();

        builder.Services.AddSingleton<IMessageConsumer<SampleMessage>, KafkaMessageConsumer<SampleMessage>>(sp =>
            new KafkaMessageConsumer<SampleMessage>(consumerConfig, topic, parallelConsumers));

        builder.Services.AddSingleton<IKafkaPlaygroundConsumerService, KafkaPlaygroundConsumerService>();


        builder.Services.AddSingleton<ISQLConnectionFactory>(sb =>
            new SQLConnectionFactory(builder.Configuration.GetConnectionString("SqlServer")?.Trim()!));

        builder.Services.AddSingleton<IOracleConnectionFactory>(sb =>
            new OracleConnectionFactory(builder.Configuration.GetConnectionString("Oracle")?.Trim()!));

        builder.Services.AddSingleton<IDbSQLRepository, SQLRepository>();
        builder.Services.AddSingleton<IDbOracleRepository, OracleRepository>();


        var app = builder.Build();
        app.Run();
    }
}

