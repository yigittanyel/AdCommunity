using AdCommunity.Core.Helpers;
using Microsoft.Extensions.Localization;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(IStringLocalizer localizer)
        : base(LocalizationHelper.Translate(localizer, "InvalidCredentialsErrorMessage"))
    {
    }
}