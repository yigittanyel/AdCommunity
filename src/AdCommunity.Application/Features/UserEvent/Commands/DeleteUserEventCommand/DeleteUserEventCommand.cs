using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserEvent.Commands.DeleteUserEventCommand;

public class DeleteUserEventCommand : IYtRequest<bool>
{
    public int Id { get; set; }
}
