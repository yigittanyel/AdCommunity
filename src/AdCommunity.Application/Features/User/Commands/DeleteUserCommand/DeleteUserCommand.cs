using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.User.Commands.DeleteUserCommand;

public class DeleteUserCommand : IYtRequest<bool>
{
    public bool IsCommand => true;
    public int Id { get; set; }
}
