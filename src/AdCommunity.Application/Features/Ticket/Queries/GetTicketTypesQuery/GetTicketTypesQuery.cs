using AdCommunity.Application.DTOs.TicketTypes;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Ticket.Queries.GetTicketQuery;

public class GetTicketTypesQuery:IYtRequest<List<TicketTypesDto>>
{
}
