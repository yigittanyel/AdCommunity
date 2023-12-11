using AdCommunity.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Exceptions;

public class NullException : ArgumentNullException
{
    public NullException(HttpContext httpContext) : base(string.Format(LocalizationHelper.TranslateWithEntity("NullExceptionErrorMessage", LocalizationHelper.GetLanguageCode(httpContext))))
    {
    }
}