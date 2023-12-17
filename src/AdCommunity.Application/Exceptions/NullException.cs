using AdCommunity.Core.Helpers;
using Microsoft.Extensions.Localization;

namespace AdCommunity.Application.Exceptions;

public class NullException : ArgumentNullException
{
    public NullException(IStringLocalizer localizer)
        : base(LocalizationHelper.Translate(localizer, "NullExceptionErrorMessage"))
    {
    }
}