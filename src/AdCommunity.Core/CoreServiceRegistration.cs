using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Request;
using AdCommunity.Core.Extensions.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace AdCommunity.Core;

public static class CoreServiceRegistration
{
    public static IServiceCollection AddYtMapper(this IServiceCollection services)
    {
        services.AddSingleton<IYtMapper, YtMapper>();
        return services;
    }

    public static IServiceCollection AddYtMeditor(this IServiceCollection services, params Assembly[] assemblies)
    {
        AddRequiredServices(services);
        RegisterServices(services, assemblies, typeof(IYtRequestHandler<,>));

        return services;
    }

    private static void AddRequiredServices(IServiceCollection services)
    {
        services.TryAdd(new ServiceDescriptor(typeof(IYtMediator), typeof(YtMediator), ServiceLifetime.Transient));
    }

    private static IServiceCollection RegisterServices(this IServiceCollection services, Assembly[] assemblies, Type handlerInterfaceType)
    {
        if (assemblies == null)
        {
            throw new ArgumentNullException(nameof(assemblies));
        }

        if (handlerInterfaceType == null)
        {
            throw new ArgumentNullException(nameof(handlerInterfaceType));
        }

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();
            var handlers = types
                .Where(x => x.GetInterfaces()
                    .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == handlerInterfaceType));

            foreach (var handler in handlers)
            {
                var interfaces = handler.GetInterfaces();
                foreach (var handlerInterface in interfaces)
                {
                    services.AddTransient(handlerInterface, handler);
                }
            }
        }

        return services;
    }
}
