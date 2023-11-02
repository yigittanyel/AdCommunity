using AdCommunity.Domain.Entities.Base;
using AdCommunity.Domain.Entities.CommunityModels;

namespace AdCommunity.Domain.Entities.UserModels;

public partial class UserCommunity : BaseEntity
{
    public int? UserId { get; protected set; }

    public int? CommunityId { get; protected set; }

    public DateTime? JoinDate { get; protected set; }

    public virtual Community? Community { get; protected set; }

    public virtual User? User { get; protected set; }

    public UserCommunity(int id, int? userId, int? communityId, DateTime? joinDate, DateTime? createdOn, Community? community, User? user)
    {
        Id = id;
        UserId = userId;
        CommunityId = communityId;
        JoinDate = joinDate;
        CreatedOn = createdOn;
        Community = community;
        User = user;
    }

    public UserCommunity(int id, int? userId, int? communityId, DateTime? joinDate, DateTime? createdOn)
    {
        Id = id;
        UserId = userId;
        CommunityId = communityId;
        JoinDate = joinDate;
        CreatedOn = createdOn;
    }

    public UserCommunity(int id)
    {
        Id = id;
    }
}
