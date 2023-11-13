using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserCommunity.Commands.DeleteUserCommunityCommand;

public class DeleteUserCommunityCommand : IYtRequest<bool>
{
    public int Id { get; set; }
}
