﻿using AdCommunity.Application.Services;
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
        var ass = Assembly.GetExecutingAssembly();
        serviceCollection.AddValidatorsFromAssembly(ass);
        #endregion

        #region Redis
        serviceCollection.AddScoped<RedisService>(sp =>
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
            Uri= new System.Uri(configuration["RabbitMQ:Url"]),
            
        };
        serviceCollection.AddSingleton(rabbitMqFactory);
        #endregion
    }
}
