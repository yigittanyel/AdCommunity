using AdCommunity.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName,HttpContext httpContext)
        : base(string.Format(LocalizationHelper.TranslateWithEntity("NotFoundErrorMessage", entityName, LocalizationHelper.GetLanguageCode(httpContext))))
    {
    }
}
