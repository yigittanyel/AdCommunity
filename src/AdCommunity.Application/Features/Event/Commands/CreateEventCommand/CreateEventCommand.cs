using AdCommunity.Application.DTOs.Event;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Event.Commands.CreateEventCommand;

public class CreateEventCommand : IYtRequest<EventCreateDto>
{
    public string EventName { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }
    public string Location { get; set; }
    public int CommunityId { get; set; }

    public CreateEventCommand(string eventName, string description, DateTime eventDate, string location, int communityId)
    {
        EventName = eventName;
        Description = description;
        EventDate = eventDate;
        Location = location;
        CommunityId = communityId;
    }
}
