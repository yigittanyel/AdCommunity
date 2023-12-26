using AdCommunity.Core.Helpers;

namespace AdCommunity.Core.BaseException;

public class YtException : Exception
{
    protected readonly LocalizationService _localizationService;

    public YtException(string key, params object[] parameters)
        : base(GetFormattedMessage(key, parameters))
    {
        _localizationService = LocalizationServiceFactory.GetLocalizationServiceInstance();
    }

    private static string GetFormattedMessage(string key, params object[] parameters)
    {
        var localizationService = LocalizationServiceFactory.GetLocalizationServiceInstance();
        return localizationService.Translate(key, parameters);
    }
}
