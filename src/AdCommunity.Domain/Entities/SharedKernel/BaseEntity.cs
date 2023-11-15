namespace AdCommunity.Domain.Entities.SharedKernel;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime? CreatedOn { get; protected set; }
}
