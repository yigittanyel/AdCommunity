using System;
using System.Collections.Generic;

namespace AdCommunity.Domain.Entities;

public partial class Event
{
    public int Id { get; set; }

    public string EventName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime EventDate { get; set; }

    public string Location { get; set; } = null!;

    public int CommunityId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual Community Community { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<UserEvent> UserEvents { get; set; } = new List<UserEvent>();
}
