using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdCommunity.Core.Helpers;

public class LocalizationServiceFactory
{
    private static readonly Lazy<LocalizationService> _localizationServiceInstance =
        new Lazy<LocalizationService>(() =>
        {
            var factory = new ResourceManagerStringLocalizerFactory(
                new OptionsWrapper<LocalizationOptions>(new LocalizationOptions()),
                new LoggerFactory()
            );

            return new LocalizationService(factory);
        });

    public static LocalizationService GetLocalizationServiceInstance()
    {
        return _localizationServiceInstance.Value;
    }
}