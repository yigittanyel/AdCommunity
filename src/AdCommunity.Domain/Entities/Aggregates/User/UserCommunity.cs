using AdCommunity.Domain.Entities.SharedKernel;

namespace AdCommunity.Domain.Entities.Aggregates.User;

public partial class UserCommunity:BaseEntity
{
    public int UserId { get; protected set; }
    public int CommunityId { get; protected set; }
    public DateTime? JoinDate { get; protected set; }
    public virtual Entities.Aggregates.Community.Community Community { get; protected set; } = null!;
    public virtual User User { get; protected set; } = null!;
    public UserCommunity(DateTime? joinDate)
    {
        JoinDate = joinDate;
        CreatedOn = DateTime.UtcNow;
    }
    public void SetDate()
    {
        CreatedOn = DateTime.UtcNow;
    }

    public void AssignUser(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        User = user;
        UserId = user.Id;
    }

    public void AssignCommunity(Entities.Aggregates.Community.Community community)
    {
        if (community is null)
            throw new ArgumentNullException(nameof(community));

        Community = community;
        CommunityId = community.Id;
    }

}
