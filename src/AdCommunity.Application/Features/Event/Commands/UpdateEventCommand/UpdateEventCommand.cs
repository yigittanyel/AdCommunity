using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Event.Commands.UpdateEventCommand;

public class UpdateEventCommand : IYtRequest<bool>
{
    public int Id { get; set; }
    public string EventName { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }
    public string Location { get; set; }
    public int CommunityId { get; set; }

    public UpdateEventCommand(int id, string eventName, string description, DateTime eventDate, string location, int communityId)
    {
        Id = id;
        EventName = eventName;
        Description = description;
        EventDate = eventDate;
        Location = location;
        CommunityId = communityId;
    }
}
