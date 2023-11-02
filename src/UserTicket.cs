using System;
using System.Collections.Generic;

namespace AdCommunity.Domain.Entities;

public partial class UserTicket
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TicketId { get; set; }

    public string? Pnr { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
