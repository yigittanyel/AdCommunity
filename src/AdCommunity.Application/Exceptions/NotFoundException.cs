using AdCommunity.Core.Helpers;

using Microsoft.Extensions.Localization;

namespace AdCommunity.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(IStringLocalizer localizer, string entityName)
        : base(LocalizationHelper.Translate(localizer, "NotFoundErrorMessage", entityName))
    {
    }
}