using AdCommunity.Application.DTOs.TicketTypes;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Ticket.Commands.CreateTicketCommand;

public class CreateTicketTypesCommand:IYtRequest<TicketTypesCreateDto>
{
    public bool IsCommand => true;
    public int CommunityEventId { get; set; }
    public int CommunityId { get; set; }
    public decimal? Price { get; set; }

    public CreateTicketTypesCommand(int communityEventId, int communityId, decimal? price)
    {
        CommunityEventId = communityEventId;
        CommunityId = communityId;
        Price = price;
    }
}
