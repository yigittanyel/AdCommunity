using AdCommunity.Application.DTOs.UserTicket;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserTicket.Commands.CreateUserTicketCommand;

public class CreateUserTicketCommand : IYtRequest<UserTicketCreateDto>
{
    public bool IsCommand => true;
    public int UserId { get; set; }
    public int TicketId { get; set; }
    public string? Pnr { get; set; }

public CreateUserTicketCommand(int userId, int ticketId, string? pnr)
    {
        UserId = userId;
        TicketId = ticketId;
        Pnr = pnr;
    }
}
