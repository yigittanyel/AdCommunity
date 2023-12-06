using AdCommunity.Application.DTOs.UserTicket;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserTicket.Queries.GetUserTicketQuery;

public class GetUserTicketQuery : IYtRequest<UserTicketDto>
{
    public bool IsCommand => false;
    public int Id { get; set; }
}