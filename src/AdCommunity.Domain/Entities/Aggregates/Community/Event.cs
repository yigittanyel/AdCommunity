using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Domain.Exceptions;

namespace AdCommunity.Domain.Entities.Aggregates.Community;


public partial class Event
{
    public int Id { get; protected set; }

    public string EventName { get; protected set; } = null!;

    public string Description { get; protected set; } = null!;

    public DateTime EventDate { get; protected set; }

    public string Location { get; protected set; } = null!;

    public int CommunityId { get; protected set; }

    public DateTime? CreatedOn { get; protected set; }

    public virtual Community Community { get; protected set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; protected set; } = new List<Ticket>();

    public virtual ICollection<UserEvent> UserEvents { get; protected set; } = new List<UserEvent>();

    public Event(string eventName, string description, DateTime eventDate, string location, int communityId)
    {
        if(string.IsNullOrWhiteSpace(eventName))
            throw new NullException(nameof(eventName));
        if (string.IsNullOrWhiteSpace(description))
            throw new NullException(nameof(description));
        if (string.IsNullOrWhiteSpace(location))
            throw new NullException(nameof(location));
        if (eventDate == null)
            throw new NullException(nameof(eventDate));
        if (communityId <= 0)
            throw new ForeignKeyException(nameof(communityId));


        EventName = eventName;
        Description = description;
        EventDate = eventDate;
        Location = location;
        CommunityId = communityId;
        CreatedOn = DateTime.UtcNow;
    }

    public void SetDate()
    {
        CreatedOn = DateTime.UtcNow;
    }
}
