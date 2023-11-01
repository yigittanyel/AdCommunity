namespace AdCommunity.Domain.Entities;

public partial class UserEvent
{
    public int Id { get; protected set; }

    public int? UserId { get; protected set; }

    public int? EventId { get; protected set; }

    public DateTime? CreatedOn { get; protected set; }

    public virtual Event? Event { get; protected set; }

    public virtual User? User { get; protected set; }

    public UserEvent(int id, int? userId, int? eventId, DateTime? createdOn, Event? @event, User? user)
    {
        Id = id;
        UserId = userId;
        EventId = eventId;
        CreatedOn = createdOn;
        Event = @event;
        User = user;
    }

    public UserEvent(int id, int? userId, int? eventId, DateTime? createdOn)
    {
        Id = id;
        UserId = userId;
        EventId = eventId;
        CreatedOn = createdOn;
    }

    public UserEvent(int id)
    {
        Id = id;
    }
}
