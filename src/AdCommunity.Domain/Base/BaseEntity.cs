namespace AdCommunity.Domain.Base;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime? CreatedOn { get; protected set; }
}
