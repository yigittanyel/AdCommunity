namespace Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException()
    {
    }
    public NotFoundException(string message)
        : base(message)
    {
    }
    public NotFoundException(string entityName, int entityId)
        : base($"{entityName} with ID {entityId} was not found.")
    {
    }
}