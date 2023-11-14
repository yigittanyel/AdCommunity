using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Domain.Entities.Base;

namespace AdCommunity.Domain.Entities.Aggregates.Community;

public partial class Event:BaseEntity
{
    public string EventName { get; protected set; } = null!;
    public string Description { get; protected set; } = null!;
    public DateTime EventDate { get; protected set; }
    public string Location { get; protected set; } = null!;
    public int CommunityId { get; protected set; }
    public virtual Community Community { get; protected set; } = null!;
    public virtual ICollection<Ticket> Tickets { get; protected set; } = new List<Ticket>();
    public virtual ICollection<UserEvent> UserEvents { get; protected set; } = new List<UserEvent>();
    public Event(string eventName, string description, DateTime eventDate, string location, int communityId)
    {
        ArgumentException.ThrowIfNullOrEmpty(eventName, nameof(eventName));
        ArgumentException.ThrowIfNullOrEmpty(description, nameof(description));
        ArgumentException.ThrowIfNullOrEmpty(location, nameof(location));

        if (communityId <= 0)
        {
            throw new ArgumentException("Community ID must be greater than 0.", nameof(communityId));
        }

        EventName = eventName;
        Description = description;
        EventDate = eventDate;
        Location = location;
        CommunityId = communityId;
        CreatedOn = DateTime.UtcNow;
    }

    public void CreateTicket(Ticket ticket) // or addticket
    {
        //check if ticket is null
        Tickets.Add(ticket);
    }

    public void SetDate()
    {
        CreatedOn = DateTime.UtcNow;
    }

    public static Event Create(string eventName, string description, DateTime eventDate, string location, int communityId)
    {
        return new Event(eventName, description, eventDate, location, communityId);
    }
}
