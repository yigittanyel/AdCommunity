using AdCommunity.Domain.Entities.Aggregates.Community;

namespace AdCommunity.Domain.Entities.Aggregates.User;

public partial class UserTicket
{
    public int Id { get; protected set; }

    public int UserId { get; protected set; }

    public int TicketId { get; protected set; }

    public string? Pnr { get; protected set; }

    public DateTime? CreatedOn { get; protected set; }

    public virtual Ticket Ticket { get; protected set; } = null!;

    public virtual User User { get; protected set; } = null!;
}
