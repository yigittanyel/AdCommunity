using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Domain.Entities.SharedKernel;

namespace AdCommunity.Domain.Entities.Aggregates.Community;

public partial class Event:BaseEntity
{
    public string EventName { get; protected set; } = null!;
    public string Description { get; protected set; } = null!;
    public DateTime EventDate { get; protected set; }
    public string Location { get; protected set; } = null!;
    public int CommunityId { get; protected set; }
    public virtual Community Community { get; protected set; } = null!;
    public virtual ICollection<TicketType> Tickets { get; protected set; } = new List<TicketType>();
    public virtual ICollection<UserEvent> UserEvents { get; protected set; } = new List<UserEvent>();
    public Event(string eventName, string description, DateTime eventDate, string location)
    {
        ArgumentException.ThrowIfNullOrEmpty(eventName, nameof(eventName));
        ArgumentException.ThrowIfNullOrEmpty(description, nameof(description));
        ArgumentException.ThrowIfNullOrEmpty(location, nameof(location));

        EventName = eventName;
        Description = description;
        EventDate = eventDate;
        Location = location;
        CreatedOn = DateTime.UtcNow;
    }

    public void AssignCommunity(Community community)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(community));

        Community = community;
        CommunityId = community.Id;
    }

    public void SetDate()
    {
        CreatedOn = DateTime.UtcNow;
    }

}
