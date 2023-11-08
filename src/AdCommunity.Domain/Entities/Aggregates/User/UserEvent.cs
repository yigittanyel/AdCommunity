using AdCommunity.Domain.Entities.Aggregates.Community;
using AdCommunity.Domain.Entities.Base;
using AdCommunity.Domain.Exceptions;

namespace AdCommunity.Domain.Entities.Aggregates.User;

public partial class UserEvent:BaseEntity
{
    public int UserId { get; protected set; }
    public int EventId { get; protected set; }
    public virtual Event Event { get; protected set; } = null!;
    public virtual User User { get; protected set; } = null!;
    public UserEvent(int userId, int eventId)
    {
        if (userId <= 0)
            throw new ForeignKeyException(nameof(userId));
        if (eventId <= 0)
            throw new ForeignKeyException(nameof(eventId));

        UserId = userId;
        EventId = eventId;
        CreatedOn = DateTime.UtcNow;
    }
    public void SetDate()
    {
        CreatedOn = DateTime.UtcNow;
    }

    public static UserEvent Create(int userId, int eventId)
    {
        return new UserEvent(userId, eventId);
    }

}
