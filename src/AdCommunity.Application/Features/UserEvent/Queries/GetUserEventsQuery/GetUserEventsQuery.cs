using AdCommunity.Application.DTOs.UserEvent;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserEvent.Queries.GetUserEventsQuery;

public class GetUserEventsQuery : IYtRequest<List<UserEventDto>>
{
    public bool IsCommand => false;
}

