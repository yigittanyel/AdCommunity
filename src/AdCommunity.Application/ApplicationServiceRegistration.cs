using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Features.User.Queries;
using AdCommunity.Application.Services;
using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdCommunity.Application;

public static class ApplicationServiceRegistration
{
    public static void AddApplicationRegistration(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<RedisService>(sp =>
        {
            return new RedisService(configuration["Redis:ConnectionString"]);
        });
    }
}
