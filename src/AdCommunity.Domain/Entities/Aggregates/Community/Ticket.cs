﻿using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Domain.Exceptions;

namespace AdCommunity.Domain.Entities.Aggregates.Community;

public partial class Ticket
{
    public int Id { get; protected set; }

    public int CommunityEventId { get; protected set; }

    public int CommunityId { get; protected set; }

    public decimal? Price { get; protected set; }

    public DateTime? CreatedOn { get; protected set; }

    public virtual Community Community { get; protected set; } = null!;

    public virtual Event CommunityEvent { get; protected set; } = null!;

    public virtual ICollection<UserTicket> UserTickets { get; protected set; } = new List<UserTicket>();

    public Ticket(int communityEventId, int communityId, decimal? price)
    {
        if (communityEventId <= 0)
            throw new ForeignKeyException(nameof(communityEventId));
        if (communityId <= 0)
            throw new ForeignKeyException(nameof(communityId));

        CommunityEventId = communityEventId;
        CommunityId = communityId;
        Price = price;
        CreatedOn = DateTime.UtcNow;
    }

}