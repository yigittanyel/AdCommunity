using System;
using System.Collections.Generic;

namespace AdCommunity.Domain.Entities;

public partial class Ticket
{
    public int Id { get; set; }

    public int? CommunityEventId { get; set; }

    public int? CommunityId { get; set; }

    public decimal? Price { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual Community? Community { get; set; }

    public virtual Event? CommunityEvent { get; set; }

    public virtual ICollection<UserTicket> UserTickets { get; set; } = new List<UserTicket>();
}
