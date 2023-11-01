using AdCommunity.Domain.Base;
using AdCommunity.Domain.Entities.UserModels;

namespace AdCommunity.Domain.Entities.CommunityModels;

public partial class Event:BaseEntity
{
    public string? EventName { get; protected set; }

    public string? Description { get; protected set; }

    public DateTime? EventDate { get; protected set; }

    public string? Location { get; protected set; }

    public int? CommunityId { get; protected set; }

    public virtual Community? Community { get; protected set; }

    public virtual ICollection<Ticket> Tickets { get; protected set; } = new List<Ticket>();

    public virtual ICollection<UserEvent> UserEvents { get; protected set; } = new List<UserEvent>();

    public Event(int id, string? eventName, string? description, DateTime? eventDate, string? location, int? communityId, DateTime? createdOn, Community? community, ICollection<Ticket> tickets, ICollection<UserEvent> userEvents)
    {
        Id = id;
        EventName = eventName;
        Description = description;
        EventDate = eventDate;
        Location = location;
        CommunityId = communityId;
        CreatedOn = createdOn;
        Community = community;
        Tickets = tickets;
        UserEvents = userEvents;
    }

    public Event(int id, string? eventName, string? description, DateTime? eventDate, string? location, int? communityId, DateTime? createdOn)
    {
        Id = id;
        EventName = eventName;
        Description = description;
        EventDate = eventDate;
        Location = location;
        CommunityId = communityId;
        CreatedOn = createdOn;
    }

    public Event(int id)
    {
        Id = id;
    }
}
