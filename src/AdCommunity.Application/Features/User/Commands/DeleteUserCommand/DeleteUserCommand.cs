using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.User.Commands.DeleteUserCommand;

public class DeleteUserCommand : IYtRequest<bool>
{
    public int Id { get; set; }
}
