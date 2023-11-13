using AdCommunity.Application.DTOs.UserTicket;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserTicket.Queries.GetUserTicketsQuery;

public class GetUserTicketsQuery : IYtRequest<List<UserTicketDto>>
{
}