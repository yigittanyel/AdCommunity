using AdCommunity.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException(string entityName, HttpContext httpContext)
        : base(string.Format(LocalizationHelper.TranslateWithEntity("AlreadyExistsErrorMessage", entityName, LocalizationHelper.GetLanguageCode(httpContext))))
    {
    }
}
