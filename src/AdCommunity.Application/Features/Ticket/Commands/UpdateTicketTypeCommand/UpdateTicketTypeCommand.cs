using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Ticket.Commands.UpdateTicketCommand;

public class UpdateTicketTypeCommand : IYtRequest<bool>
{
    public bool IsCommand => true;
    public int Id { get; set; }
    public int CommunityEventId { get; set; }
    public int CommunityId { get; set; }
    public decimal? Price { get; set; }

    public UpdateTicketTypeCommand(int id, int communityEventId, int communityId, decimal? price)
    {
        Id = id;
        CommunityEventId = communityEventId;
        CommunityId = communityId;
        Price = price;
    }
}
