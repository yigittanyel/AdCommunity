using AdCommunity.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdCommunity.Application;

public static class ApplicationServiceRegistration
{
    public static void AddApplicationRegistration(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddSingleton<RedisService>(sp =>
        {
            return new RedisService(configuration["Redis:ConnectionString"]);
        });

    }
}
