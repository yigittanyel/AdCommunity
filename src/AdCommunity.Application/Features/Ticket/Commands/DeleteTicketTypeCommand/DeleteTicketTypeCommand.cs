using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Ticket.Commands.DeleteTicketCommand;

public class DeleteTicketCommand : IYtRequest<bool>
{
    public int Id { get; set; }
}
