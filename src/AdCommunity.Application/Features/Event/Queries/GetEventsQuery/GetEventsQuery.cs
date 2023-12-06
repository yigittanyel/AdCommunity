using AdCommunity.Application.DTOs.Event;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Event.Queries.GetEventsQuery;

public class GetEventsQuery : IYtRequest<List<EventDto>>
{
    public bool IsCommand => false;
}
