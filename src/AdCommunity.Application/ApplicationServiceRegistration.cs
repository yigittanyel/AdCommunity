using AdCommunity.Application.Features.Community.Commands.CreateCommunityCommand;
using AdCommunity.Application.Features.PipelineExample;
using AdCommunity.Application.Services.ElasticSearch;
using AdCommunity.Application.Services.Jwt;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Application.Validators;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using FluentValidation;
using FluentValidation.AspNetCore;
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

        serviceCollection.AddTransient(typeof(IYtPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    }

    //public static void AddTransactionalDecorators(this IServiceCollection services)
    //{
    //    services.Scan(scan => scan
    //        .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
    //        .AddClasses(classes => classes.AssignableTo(typeof(IYtRequestHandler<,>)))
    //        .AsImplementedInterfaces()
    //        .WithScopedLifetime());

    //    var handlerTypes = AppDomain.CurrentDomain.GetAssemblies()
    //        .SelectMany(s => s.GetTypes())
    //        .Where(p => typeof(Core.CustomMediator.Interfaces.IYtRequestHandler<,>).IsAssignableFrom(p) && !p.IsInterface);

    //    foreach (var handlerType in handlerTypes)
    //    {
    //        var interfaceType = handlerType.GetInterfaces()
    //            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IYtRequestHandler<,>));

    //        if (interfaceType != null)
    //        {
    //            var requestType = interfaceType.GetGenericArguments()[0];
    //            var responseType = interfaceType.GetGenericArguments()[1];
    //            var decoratorType = typeof(TransactionalRequestHandlerDecorator<,>).MakeGenericType(requestType, responseType);

    //            services.Decorate(interfaceType, (inner, provider) =>
    //            {
    //                var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
    //                var decorator = Activator.CreateInstance(decoratorType, inner, unitOfWork);
    //                return decorator;
    //            });
    //        }
    //    }
    //}
}
