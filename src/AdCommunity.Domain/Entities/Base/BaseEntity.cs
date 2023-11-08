namespace AdCommunity.Domain.Entities.Base;

public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public DateTime? CreatedOn { get; protected set; }
}
