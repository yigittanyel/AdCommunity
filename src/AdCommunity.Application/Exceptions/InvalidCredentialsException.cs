namespace AdCommunity.Application.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(string errorMessage) : base(errorMessage)
    {
    }
}
