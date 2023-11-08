using AdCommunity.Domain.Entities.Aggregates.Community;
using AdCommunity.Domain.Entities.Base;
using AdCommunity.Domain.Exceptions;

namespace AdCommunity.Domain.Entities.Aggregates.User;

public partial class UserTicket:BaseEntity
{
    public int UserId { get; protected set; }
    public int TicketId { get; protected set; }
    public string? Pnr { get; protected set; }
    public virtual Ticket Ticket { get; protected set; } = null!;
    public virtual User User { get; protected set; } = null!;
    public UserTicket(int userId, int ticketId, string? pnr)
    {
        if (userId <= 0)
            throw new ForeignKeyException(nameof(userId));
        if (ticketId <= 0)
            throw new ForeignKeyException(nameof(ticketId));

        UserId = userId;
        TicketId = ticketId;
        Pnr = pnr;
        CreatedOn = DateTime.UtcNow;
    }
    public void SetDate()
    {
        CreatedOn = DateTime.UtcNow;
    }
    public static UserTicket Create(int userId, int ticketId, string? pnr)
    {
        return new UserTicket(userId, ticketId, pnr);
    }
}
