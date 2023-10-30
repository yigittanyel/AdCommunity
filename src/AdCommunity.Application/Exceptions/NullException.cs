namespace AdCommunity.Application.Exceptions;

public class NullException : ArgumentNullException
{
    public NullException() : base()
    {
    }

    public NullException(string paramName) : base(paramName)
    {
    }

    public NullException(string message, Exception ex) : base(message, ex)
    {
    }
}