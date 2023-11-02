using AdCommunity.Domain.Entities.Aggregates.Community;
using AdCommunity.Domain.Exceptions;

namespace AdCommunity.Domain.Entities.Aggregates.User;

public partial class UserEvent
{
    public int Id { get; protected set; }

    public int UserId { get; protected set; }

    public int EventId { get; protected set; }

    public DateTime? CreatedOn { get; protected set; }

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
}
