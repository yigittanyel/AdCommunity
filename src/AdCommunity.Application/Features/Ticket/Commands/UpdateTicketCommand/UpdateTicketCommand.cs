using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Ticket.Commands.UpdateTicketCommand;

public class UpdateTicketCommand : IYtRequest<bool>
{
    public int Id { get; set; }
    public int CommunityEventId { get; set; }
    public int CommunityId { get; set; }
    public decimal? Price { get; set; }

    public UpdateTicketCommand(int id, int communityEventId, int communityId, decimal? price)
    {
        Id = id;
        CommunityEventId = communityEventId;
        CommunityId = communityId;
        Price = price;
    }
}
