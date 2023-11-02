namespace AdCommunity.Domain.Entities.Aggregates.User;

public partial class UserCommunity
{
    public int Id { get; protected set; }

    public int UserId { get; protected set; }

    public int CommunityId { get; protected set; }

    public DateTime? JoinDate { get; protected set; }

    public DateTime? CreatedOn { get; protected set; }

    public virtual Entities.Aggregates.Community.Community Community { get; protected set; } = null!;

    public virtual User User { get; protected set; } = null!;
}
