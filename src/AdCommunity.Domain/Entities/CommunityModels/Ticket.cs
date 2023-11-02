using AdCommunity.Domain.Entities.Base;
using AdCommunity.Domain.Entities.UserModels;

namespace AdCommunity.Domain.Entities.CommunityModels;

public partial class Ticket:BaseEntity
{
    public int? CommunityEventId { get; protected set; }

    public int? CommunityId { get; protected set; }

    public decimal? Price { get; protected set; }

    public virtual Community? Community { get; protected set; }

    public virtual Event? CommunityEvent { get; protected set; }

    public virtual ICollection<UserTicket> UserTickets { get; protected set; } = new List<UserTicket>();

    public Ticket(int id, int? communityEventId, int? communityId, decimal? price, DateTime? createdOn, Community? community, Event? communityEvent, ICollection<UserTicket> userTickets)
    {
        Id = id;
        CommunityEventId = communityEventId;
        CommunityId = communityId;
        Price = price;
        CreatedOn = createdOn;
        Community = community;
        CommunityEvent = communityEvent;
        UserTickets = userTickets;
    }

    public Ticket(int id, int? communityEventId, int? communityId, decimal? price, DateTime? createdOn)
    {
        Id = id;
        CommunityEventId = communityEventId;
        CommunityId = communityId;
        Price = price;
        CreatedOn = createdOn;
    }

    public Ticket(int id)
    {
        Id = id;
    }

}
