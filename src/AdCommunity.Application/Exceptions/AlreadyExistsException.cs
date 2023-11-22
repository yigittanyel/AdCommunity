namespace AdCommunity.Application.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException(string entityName)
        : base($"{entityName} is already exist.")
    {
    }

    public AlreadyExistsException(string entityName, Exception innerException)
        : base($"{entityName} is already exist.", innerException)
    {
    }
}
