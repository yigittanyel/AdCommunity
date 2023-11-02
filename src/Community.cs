using System;
using System.Collections.Generic;

namespace AdCommunity.Domain.Entities;

public partial class Community
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Tags { get; set; }

    public string? Location { get; set; }

    public string? Organizators { get; set; }

    public string? Website { get; set; }

    public string? Facebook { get; set; }

    public string? Twitter { get; set; }

    public string? Instagram { get; set; }

    public string? Github { get; set; }

    public string? Medium { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<UserCommunity> UserCommunities { get; set; } = new List<UserCommunity>();
}
