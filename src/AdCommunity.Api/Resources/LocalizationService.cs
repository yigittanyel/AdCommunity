using Microsoft.Extensions.Localization;
using System.Reflection;

namespace AdCommunity.Api.Resources;

public class LocalizationService
{
    private readonly IStringLocalizer _localizer;

    public LocalizationService(IStringLocalizerFactory factory)
    {
        var type = typeof(ApplicationResource);
        var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
        _localizer = factory.Create(nameof(ApplicationResource), assemblyName.Name);
    }

    public LocalizedString GetKey(string key)
    {
        return _localizer[key];
    }
}