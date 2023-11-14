using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserTicket.Commands.DeleteUserTicketCommand;
public class DeleteUserTicketCommand : IYtRequest<bool>
{
    public int Id { get; set; }
}
