using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserCommunity.Commands.DeleteUserCommunityCommand;

public class DeleteUserCommunityCommand : IYtRequest<bool>
{
    public bool IsCommand => true;
    public int Id { get; set; }
}
