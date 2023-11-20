using AdCommunity.Application.DTOs.UserEvent;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserEvent.Queries.GetUserEventQuery;

public class GetUserEventQuery:IYtRequest<UserEventDto>
{
    public int Id { get; set; }
}
