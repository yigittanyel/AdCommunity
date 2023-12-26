using AdCommunity.Core.BaseException;

namespace AdCommunity.Application.Exceptions;
public class AlreadyExistsException : YtException
{
    public AlreadyExistsException(string entityName)
        : base("AlreadyExistsErrorMessage", entityName)
    {
    }
}