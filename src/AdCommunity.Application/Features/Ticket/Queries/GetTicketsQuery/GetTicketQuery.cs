using AdCommunity.Application.DTOs.Ticket;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Ticket.Queries.GetTicketsQuery;

public class GetTicketQuery : IYtRequest<TicketDto>
{
    public int Id { get; set; }
}

