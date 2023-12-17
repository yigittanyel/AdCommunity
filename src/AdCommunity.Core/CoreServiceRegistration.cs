using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Reflection;

namespace AdCommunity.Core;

public static class CoreServiceRegistration
{
    public static IServiceCollection AddLocalizationOperations(this IServiceCollection services)
    {
        services.AddSingleton<LocalizationService>();

        services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
        services.Configure<RequestLocalizationOptions>(opt =>
        {
            var cultures = new List<CultureInfo>()
            {
                new CultureInfo("tr-TR"),
                new CultureInfo("en-US")
            };

            opt.SupportedCultures = cultures;
            opt.SupportedUICultures = cultures;

            opt.RequestCultureProviders = new List<IRequestCultureProvider>()
            {
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider(),
                new AcceptLanguageHeaderRequestCultureProvider()
            };
        });

        return services;
    }

    public static IServiceCollection AddYtMapper(this IServiceCollection services)
    {
        services.AddScoped<IYtMapper, YtMapper>();
        return services;
    }

    public static IServiceCollection AddYtMeditor(this IServiceCollection services, params Assembly[] assemblies)
    {
        RegisterServices(services, assemblies);

        return services;
    }

    private static IServiceCollection RegisterServices(this IServiceCollection services, Assembly[] assemblies)
    {
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IYtRequestHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IYtPipelineBehavior<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IYtMediator)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(TransactionalRequestHandlerDecorator<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Decorate(typeof(IYtRequestHandler<,>), typeof(TransactionalRequestHandlerDecorator<,>));

        return services;
    }
}
