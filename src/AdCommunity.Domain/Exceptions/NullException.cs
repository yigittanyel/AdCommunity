namespace AdCommunity.Domain.Exceptions;

public class NullException : ArgumentNullException
{
    public NullException() : base()
    {
    }

    public NullException(string paramName)
           : base($"Value cannot be null. (Parameter name: {paramName})")
    {
    }

    public NullException(string message, Exception ex) : base(message, ex)
    {
    }
}