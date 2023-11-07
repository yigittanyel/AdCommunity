using AdCommunity.Application.DTOs.Event;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Event.Queries.GetEventQuery;

public class GetEventQuery : IYtRequest<EventDto>
{
    public int Id { get; set; }
}
