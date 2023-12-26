using AdCommunity.Core.BaseException;

public class InvalidCredentialsException : YtException
{
    public InvalidCredentialsException()
        : base("InvalidCredentialsErrorMessage")
    {
    }
}