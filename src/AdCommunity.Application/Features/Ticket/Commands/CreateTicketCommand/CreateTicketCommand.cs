using AdCommunity.Application.DTOs.Ticket;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Ticket.Commands.CreateTicketCommand;

public class CreateTicketCommand:IYtRequest<TicketCreateDto>
{
    public int CommunityEventId { get; set; }
    public int CommunityId { get; set; }
    public decimal? Price { get; set; }

    public CreateTicketCommand(int communityEventId, int communityId, decimal? price)
    {
        CommunityEventId = communityEventId;
        CommunityId = communityId;
        Price = price;
    }
}
