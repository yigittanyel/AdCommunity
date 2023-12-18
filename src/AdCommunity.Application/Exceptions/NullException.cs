using AdCommunity.Core.Helpers;

namespace AdCommunity.Application.Exceptions;
public class NullException : ArgumentNullException
{
    public NullException(LocalizationService localizationService)
        : base(localizationService.Translate("NullExceptionErrorMessage"))
    {
    }
}
