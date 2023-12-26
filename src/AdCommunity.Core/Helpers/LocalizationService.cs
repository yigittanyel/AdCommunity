using Microsoft.Extensions.Localization;
using System.Reflection;

namespace AdCommunity.Core.Helpers;
public class LocalizationService
{
    private readonly IStringLocalizer _localizer;

    public LocalizationService(IStringLocalizerFactory factory)
    {
        var type = typeof(ApplicationResource);
        var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
        _localizer = factory.Create(nameof(ApplicationResource), assemblyName.Name);
    }

    public string Translate(string key, params object[] parameters)
    {
        var translation = _localizer[key];
        return translation ?? string.Format(key, parameters);
    }
}

