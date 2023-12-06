using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Community.Commands.DeleteCommunityCommand;

public class DeleteCommunityCommand : IYtRequest<bool>
{
    public int Id { get; set; }
    public bool IsCommand => true;

}
