using AdCommunity.Core.BaseException;

namespace AdCommunity.Domain.Exceptions;

public class NullException : YtException
{
    public NullException(string entityName)
        : base("NullErrorMessage", entityName)
    {
    }
}