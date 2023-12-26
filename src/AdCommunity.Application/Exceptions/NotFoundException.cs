using AdCommunity.Core.BaseException;

namespace AdCommunity.Application.Exceptions;
public class NotFoundException : YtException
{
    public NotFoundException(string entityName)
        : base("NotFoundErrorMessage", entityName)
    {
    }
}