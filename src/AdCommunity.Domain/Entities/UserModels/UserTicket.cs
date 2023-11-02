using AdCommunity.Domain.Entities.Base;
using AdCommunity.Domain.Entities.CommunityModels;

namespace AdCommunity.Domain.Entities.UserModels;

public partial class UserTicket:BaseEntity
{
    public int? UserId { get; protected set; }

    public int? TicketId { get; protected set; }

    public string? Pnr { get; protected set; }

    public virtual Ticket? Ticket { get; protected set; }

    public virtual User? User { get; protected set; }

    public UserTicket(int id, int? userId, int? ticketId, string? pnr, DateTime? createdOn, Ticket? ticket, User? user)
    {
        Id = id;
        UserId = userId;
        TicketId = ticketId;
        Pnr = pnr;
        CreatedOn = createdOn;
        Ticket = ticket;
        User = user;
    }

    public UserTicket(int id, int? userId, int? ticketId, string? pnr, DateTime? createdOn)
    {
        Id = id;
        UserId = userId;
        TicketId = ticketId;
        Pnr = pnr;
        CreatedOn = createdOn;
    }

    public UserTicket(int id)
    {
        Id = id;
    }
}
