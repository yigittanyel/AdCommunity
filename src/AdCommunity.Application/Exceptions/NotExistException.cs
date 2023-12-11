using AdCommunity.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Exceptions;

public class NotExistException : Exception
{
    public NotExistException(string entityName, HttpContext httpContext)
        : base(string.Format(LocalizationHelper.TranslateWithEntity("NotExistErrorMessage", entityName, LocalizationHelper.GetLanguageCode(httpContext))))
    {
    }
}
