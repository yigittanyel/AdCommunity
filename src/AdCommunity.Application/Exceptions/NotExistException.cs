using AdCommunity.Core.BaseException;

namespace AdCommunity.Application.Exceptions;
public class NotExistException : YtException
{
    public NotExistException(string entityName)
        : base("NotExistErrorMessage", entityName)
    {
    }
}