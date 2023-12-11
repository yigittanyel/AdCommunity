using AdCommunity.Application.Features.PipelineExample;
using AdCommunity.Application.Services.ElasticSearch;
using AdCommunity.Application.Services.Jwt;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMediator.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        serviceCollection.AddScoped<IElasticSearchService, ElasticSearchService>();
        #endregion

        #region Pipeline Behavior
        serviceCollection.AddTransient(typeof(IYtPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        #endregion

        serviceCollection.AddHttpContextAccessor();
    }
}
