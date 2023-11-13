using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserCommunity.Commands.UpdateUserCommunityCommand;

public class UpdateUserCommunityCommand : IYtRequest<bool>
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CommunityId { get; set; }
    public DateTime? JoinDate { get; set; }

    public UpdateUserCommunityCommand(int id, int userId, int communityId, DateTime? joinDate)
    {
        Id = id;
        UserId = userId;
        CommunityId = communityId;
        JoinDate = joinDate;
    }
}
