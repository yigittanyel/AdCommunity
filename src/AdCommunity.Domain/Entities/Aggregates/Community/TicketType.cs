using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Domain.Entities.SharedKernel;

namespace AdCommunity.Domain.Entities.Aggregates.Community;

public partial class TicketType:BaseEntity
{
    public int CommunityEventId { get; protected set; }
    public int CommunityId { get; protected set; }
    public decimal? Price { get; protected set; }
    public virtual Community Community { get; protected set; } = null!;
    public virtual Event CommunityEvent { get; protected set; } = null!;
    public virtual ICollection<UserTicket> UserTickets { get; protected set; } = new List<UserTicket>();
    public TicketType(decimal? price)
    {
        if(price < 0)
            throw new ArgumentException("Price cannot be less than zero.", nameof(price));

        Price = price;
        CreatedOn = DateTime.UtcNow;
    }
    public void SetDate()
    {
        CreatedOn = DateTime.UtcNow;
    }

    public void AssignCommunity(Community community)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(community));

        Community = community;
        CommunityId = community.Id;
    }

    public void AssignEvent(Event @event)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(@event));

        CommunityEvent = @event;
        CommunityEventId = @event.Id;
    }


}
