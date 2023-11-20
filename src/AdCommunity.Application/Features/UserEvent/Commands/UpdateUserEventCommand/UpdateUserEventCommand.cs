using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserEvent.Commands.UpdateUserEventCommand;

public class UpdateUserEventCommand : IYtRequest<bool>
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }

    public UpdateUserEventCommand(int id, int userId, int eventId)
    {
        Id = id;
        UserId = userId;
        EventId = eventId;
    }
}
