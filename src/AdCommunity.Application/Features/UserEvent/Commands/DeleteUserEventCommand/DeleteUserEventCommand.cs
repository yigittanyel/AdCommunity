using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserEvent.Commands.DeleteUserEventCommand;

public class DeleteUserEventCommand : IYtRequest<bool>
{
    public bool IsCommand => true;
    public int Id { get; set; }
}
