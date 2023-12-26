using AdCommunity.Core.BaseException;

namespace AdCommunity.Application.Exceptions;

public class NullException : YtException
{
    public NullException(string entityName)
        : base("NullExceptionErrorMessage", entityName)
    {
    }
}