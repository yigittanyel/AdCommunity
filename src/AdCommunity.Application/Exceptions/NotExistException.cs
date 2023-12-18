using AdCommunity.Core.Helpers;

namespace AdCommunity.Application.Exceptions;

public class NotExistException : Exception
{
    public NotExistException(LocalizationService localizationService, string entityName)
        : base(localizationService.Translate("NotExistErrorMessage", entityName))
    {
    }
}