using AdCommunity.Application.DTOs.UserEvent;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserEvent.Commands.CreateUserEventCommand;

public class CreateUserEventCommand : IYtRequest<UserEventCreateDto>
{
    public bool IsCommand => true;
    public int UserId { get; set; }
    public int EventId { get; set; }

    public CreateUserEventCommand(int userId, int eventId)
    {
        UserId = userId;
        EventId = eventId;
    }
}
