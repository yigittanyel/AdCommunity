using AdCommunity.Core.Helpers;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(LocalizationService localizationService)
        : base(localizationService.Translate("InvalidCredentialsErrorMessage"))
    {
    }
}