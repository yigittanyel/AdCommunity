using System;
using System.Collections.Generic;

namespace AdCommunity.Domain.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public string? Username { get; set; }

    public string? Website { get; set; }

    public string? Facebook { get; set; }

    public string? Twitter { get; set; }

    public string? Instagram { get; set; }

    public string? Github { get; set; }

    public string? Medium { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? HashedPassword { get; set; }

    public virtual ICollection<UserCommunity> UserCommunities { get; set; } = new List<UserCommunity>();

    public virtual ICollection<UserEvent> UserEvents { get; set; } = new List<UserEvent>();

    public virtual ICollection<UserTicket> UserTickets { get; set; } = new List<UserTicket>();
}
