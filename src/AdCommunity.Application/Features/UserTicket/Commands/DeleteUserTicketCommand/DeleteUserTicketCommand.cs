using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserTicket.Commands.DeleteUserTicketCommand;
public class DeleteUserTicketCommand : IYtRequest<bool>
{
    public bool IsCommand => true;
    public int Id { get; set; }
}
