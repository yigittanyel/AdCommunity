using AdCommunity.Domain.Entities.SharedKernel;
using AdCommunity.Domain.Exceptions;

namespace AdCommunity.Domain.Entities.Aggregates.User;

public partial class UserCommunity:BaseEntity
{
    public int UserId { get; protected set; }
    public int CommunityId { get; protected set; }
    public DateTime? JoinDate { get; protected set; }
    public virtual Entities.Aggregates.Community.Community Community { get; protected set; } = null!;
    public virtual User User { get; protected set; } = null!;
    public UserCommunity(int userId, int communityId, DateTime? joinDate)
    {
        if(userId <= 0)
            throw new ForeignKeyException(nameof(userId));
        if (communityId <= 0)
            throw new ForeignKeyException(nameof(communityId));

        UserId = userId;
        CommunityId = communityId;
        JoinDate = joinDate;
        CreatedOn = DateTime.UtcNow;
    }
    public void SetDate()
    {
        CreatedOn = DateTime.UtcNow;
    }
    public static UserCommunity Create(int userId, int communityId, DateTime? joinDate)
    {
        return new UserCommunity(userId, communityId, joinDate);
    }
}
