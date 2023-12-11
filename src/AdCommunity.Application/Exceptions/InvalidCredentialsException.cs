using AdCommunity.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(HttpContext httpContext) : base(string.Format(LocalizationHelper.TranslateWithEntity("InvalidCredentialsErrorMessage", LocalizationHelper.GetLanguageCode(httpContext))))
    {
    }
}
