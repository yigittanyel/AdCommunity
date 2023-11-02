namespace AdCommunity.Domain.Exceptions;

public class ForeignKeyException : ArgumentException
{
    public ForeignKeyException() : base()
    {
    }

    public ForeignKeyException(string paramName)
           : base($"Foreign key value must be not equal 0 and greater than 0. (Parameter name: {paramName})")
    {
    }

    public ForeignKeyException(string message, Exception ex) : base(message, ex)
    {
    }
}