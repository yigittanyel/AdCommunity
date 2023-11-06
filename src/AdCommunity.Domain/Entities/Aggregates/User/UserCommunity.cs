using AdCommunity.Domain.Exceptions;

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

    public UserCommunity(int userId, int communityId, DateTime? joinDate)
    {
        if(userId <= 0)
            throw new ForeignKeyException(nameof(userId));
        if (communityId <= 0)
            throw new ForeignKeyException(nameof(communityId));
        if (Id <= 0 || Id == null)
            throw new Exception("Id cannot be null, zero or less than zero.");

        UserId = userId;
        CommunityId = communityId;
        JoinDate = joinDate;
        CreatedOn = DateTime.UtcNow;
    }
}
