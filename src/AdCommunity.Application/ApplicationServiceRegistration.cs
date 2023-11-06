using AdCommunity.Application.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AdCommunity.Application;

public static class ApplicationServiceRegistration
{
    public static void AddApplicationRegistration(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var ass = Assembly.GetExecutingAssembly();

        serviceCollection.AddValidatorsFromAssembly(ass);

        serviceCollection.AddScoped<RedisService>(sp =>
        {
            return new RedisService(configuration["Redis:ConnectionString"]);
        });

        serviceCollection.AddScoped<IJwtService, JwtService>();
    }
}
