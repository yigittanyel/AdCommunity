using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Event.Commands.DeleteEventCommand;

public class DeleteEventCommand : IYtRequest<bool>
{
    public int Id { get; set; }
}
