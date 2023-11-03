using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator;
using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AdCommunity.Core;

public static class CoreServiceRegistration
{
    public static IServiceCollection AddYtMapper(this IServiceCollection services)
    {
        services.AddSingleton<IYtMapper, YtMapper>();
        return services;
    }

    public static IServiceCollection AddCustomMediator(this IServiceCollection services, Assembly[] assemblies)
    {
        var types = assemblies.SelectMany(i => i.GetTypes()).Where(i => !i.IsInterface);

        var requestHandlers = types
            .Where(i => IsAssignableToGenericType(i, typeof(IYtRequestHandler<,>)))
            .ToList();

        foreach (var handler in requestHandlers)
        {
            var handlerInterface = handler.GetInterfaces().FirstOrDefault();
            var requestType = handlerInterface.GetGenericArguments()[0];
            var responseType = handlerInterface.GetGenericArguments()[1];

            var genericType = typeof(IYtRequestHandler<,>).MakeGenericType(requestType, responseType);

            services.AddTransient(genericType, handler);
        }

        services.AddSingleton<IYtMediator, YtMediator>();

        return services;
    }

    public static IServiceProvider UseCustomMediator(this IServiceProvider serviceProvider)
    {
        CustomMediator.YtServiceProvider.SetInstance(serviceProvider);
        return serviceProvider;
    }



    private static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        bool IsGeneric(Type _givenType, Type _genericType)
        {
            return _givenType.IsGenericType && _givenType.GetGenericTypeDefinition() == _genericType;
        }

        var interfaceTypes = givenType.GetInterfaces();

        foreach (var it in interfaceTypes)
        {
            if (IsGeneric(it, genericType))
                return true;
        }

        if (IsGeneric(givenType, genericType))
            return true;

        Type baseType = givenType.BaseType;
        if (baseType == null) return false;

        return IsAssignableToGenericType(baseType, genericType);
    }
}
