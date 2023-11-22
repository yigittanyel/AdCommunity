namespace AdCommunity.Application.Exceptions;

public class NotExistException : Exception
{
    public NotExistException(string entityName)
        : base($"{entityName} does not exist.")
    {
    }

    public NotExistException(string entityName, Exception innerException)
        : base($"{entityName} does not exist.", innerException)
    {
    }
}
