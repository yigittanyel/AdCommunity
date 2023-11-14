using AdCommunity.Application.DTOs.TicketTypes;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Ticket.Queries.GetTicketsQuery;

public class GetTicketTypeQuery : IYtRequest<TicketTypesDto>
{
    public int Id { get; set; }
}

