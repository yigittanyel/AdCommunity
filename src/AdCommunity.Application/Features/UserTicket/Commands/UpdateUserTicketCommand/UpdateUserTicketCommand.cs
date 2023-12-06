using AdCommunity.Application.DTOs.UserTicket;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserTicket.Commands.UpdateUserTicketCommand;

public class UpdateUserTicketCommand : IYtRequest<bool>
{
    public bool IsCommand => true;
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TicketId { get; set; }
    public string? Pnr { get; set; }

    public UpdateUserTicketCommand(int id,int userId, int ticketId, string? pnr)
    {
        Id = id;
        UserId = userId;
        TicketId = ticketId;
        Pnr = pnr;
    }
}