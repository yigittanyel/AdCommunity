using AdCommunity.Core.BaseException;

namespace AdCommunity.Domain.Exceptions;

public class ForeignKeyException : YtException
{
    public ForeignKeyException(string entityName)
        : base("ForeignKeyErrorMessage", entityName)
    {
    }
}