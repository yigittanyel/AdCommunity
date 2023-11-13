using AdCommunity.Application.DTOs.UserCommunity;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserCommunity.Commands.CreateUserCommunityCommand;

public class CreateUserCommunityCommand : IYtRequest<UserCommunityCreateDto>
{
    public int UserId { get; set; }
    public int CommunityId { get; set; }
    public DateTime? JoinDate { get; set; }

    public CreateUserCommunityCommand(int userId, int communityId, DateTime? joinDate)
    {
        UserId = userId;
        CommunityId = communityId;
        JoinDate = joinDate;
    }
}
