using AdCommunity.Domain.Entities.Aggregates.Community;
using AdCommunity.Domain.Exceptions;

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

    public UserTicket(int userId, int ticketId, string? pnr)
    {
        if (userId <= 0)
            throw new ForeignKeyException(nameof(userId));
        if (ticketId <= 0)
            throw new ForeignKeyException(nameof(ticketId));
        if (Id <= 0 || Id == null)
            throw new Exception("Id cannot be null, zero or less than zero.");

        UserId = userId;
        TicketId = ticketId;
        Pnr = pnr;
        CreatedOn = DateTime.UtcNow;
    }
}
