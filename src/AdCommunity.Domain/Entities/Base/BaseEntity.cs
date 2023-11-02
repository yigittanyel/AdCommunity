namespace AdCommunity.Domain.Entities.Base;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime? CreatedOn { get; protected set; }
}
