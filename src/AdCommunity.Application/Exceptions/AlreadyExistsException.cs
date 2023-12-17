using AdCommunity.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace AdCommunity.Application.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException(IStringLocalizer localizer, string entityName)
        : base(LocalizationHelper.Translate(localizer, "AlreadyExistsErrorMessage", entityName))
    {
    }
}
