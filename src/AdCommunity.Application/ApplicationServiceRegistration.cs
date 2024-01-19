using AdCommunity.Application.Features.PipelineExample;
using AdCommunity.Application.Services.ElasticSearch;
using AdCommunity.Application.Services.Jwt;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Application.Services.Reporting;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Entities.Aggregates.Community;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using RabbitMQ.Client;
using System.Reflection;

namespace AdCommunity.Application;

public static class ApplicationServiceRegistration
{
    public static void AddApplicationRegistration(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        #region FluentValidation
        serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        #endregion

        #region Redis
        serviceCollection.AddScoped<IRedisService, RedisService>(sp =>
        {
            return new RedisService(configuration["Redis:ConnectionString"]);
        });
        #endregion

        #region JWT
        serviceCollection.AddScoped<IJwtService, JwtService>();
        #endregion

        #region RABBITMQ
        var rabbitMqFactory = new ConnectionFactory()
        {
            Port = Convert.ToInt32(configuration["RabbitMQ:Port"]),
            UserName = configuration["RabbitMQ:UserName"],
            Password = configuration["RabbitMQ:Password"],
            Uri = new System.Uri(configuration["RabbitMQ:Url"]),

        };
        serviceCollection.AddSingleton(rabbitMqFactory);

        serviceCollection.AddScoped<IMessageBrokerService, MessageBrokerService>();
        #endregion

        #region ELASTICSEARCH
        serviceCollection.AddSingleton<IElasticClient>(provider =>
        {
            var settings = new ConnectionSettings(new Uri(configuration["ElasticSearchConfig:Uri"]))
                .DefaultIndex(configuration["ElasticSearchConfig:DefaultIndex"]);
            return new ElasticClient(settings);
        });

        serviceCollection.AddSingleton(typeof(IElasticSearchService<>), typeof(ElasticSearchService<>));
        serviceCollection.AddScoped<IElasticSearchService<Community>, ElasticSearchService<Community>>();

        #endregion

        #region Pipeline Behavior
        serviceCollection.AddTransient(typeof(IYtPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        #endregion

        #region Reporting
        serviceCollection.AddScoped<IReportService, ReportService>();
        #endregion

        serviceCollection.AddHttpContextAccessor();
    }
}
