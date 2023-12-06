using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Ticket.Commands.DeleteTicketCommand;

public class DeleteTicketCommand : IYtRequest<bool>
{
    public bool IsCommand => true;
    public int Id { get; set; }
}
