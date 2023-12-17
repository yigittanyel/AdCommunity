using AdCommunity.Core.Helpers;

using Microsoft.Extensions.Localization;

namespace AdCommunity.Application.Exceptions;

public class NotExistException : Exception
{
    public NotExistException(IStringLocalizer localizer, string entityName)
        : base(LocalizationHelper.Translate(localizer, "NotExistErrorMessage", entityName))
    {
    }
}