using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Domain.Entities.SharedKernel;
using AdCommunity.Domain.Exceptions;

namespace AdCommunity.Domain.Entities.Aggregates.Community;

public partial class TicketType:BaseEntity
{
    public int CommunityEventId { get; protected set; }
    public int CommunityId { get; protected set; }
    public decimal? Price { get; protected set; }
    public virtual Community Community { get; protected set; } = null!;
    public virtual Event CommunityEvent { get; protected set; } = null!;
    public virtual ICollection<UserTicket> UserTickets { get; protected set; } = new List<UserTicket>();
    public TicketType(int communityEventId, int communityId, decimal? price)
    {
        if (communityEventId <= 0)
            throw new ForeignKeyException(nameof(communityEventId));
        if (communityId <= 0)
            throw new ForeignKeyException(nameof(communityId));
        if(price < 0)
            throw new ArgumentException("Price cannot be less than zero.", nameof(price));

        CommunityEventId = communityEventId;
        CommunityId = communityId;
        Price = price;
        CreatedOn = DateTime.UtcNow;
    }
    public void SetDate()
    {
        CreatedOn = DateTime.UtcNow;
    }

    public static TicketType Create(int communityEventId, int communityId, decimal? price)
    {
        return new TicketType(communityEventId, communityId, price);
    }
}
