using AdCommunity.Core.Helpers;

namespace AdCommunity.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(LocalizationService localizationService,string entityName)
        : base(localizationService.Translate("NotFoundErrorMessage", entityName))
    {
    }
}