using AdCommunity.Core.Helpers;

namespace AdCommunity.Application.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException(LocalizationService localizationService, string entityName)
        : base(localizationService.Translate("AlreadyExistsErrorMessage", entityName))
    {
    }
}
