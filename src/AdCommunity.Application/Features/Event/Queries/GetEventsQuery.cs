using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Helpers;
using AdCommunity.Application.Services;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Event.Queries;

public class GetEventsQuery:IYtRequest<List<EventDto>>
{
}

public class GetEventsQueryHandler : IYtRequestHandler<GetEventsQuery, List<EventDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly RedisService _redisService;

    public GetEventsQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, RedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<List<EventDto>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "events";

        var eventsDto = CacheHelper.GetFromCache<List<EventDto>>(_redisService, cacheKey);

        if (eventsDto == null)
        {
            var events = await _unitOfWork.EventRepository.GetAllAsync(cancellationToken);

            if (events == null || !events.Any())
            {
                throw new NotFoundException("Event");
            }

            eventsDto = _mapper.MapList<AdCommunity.Domain.Entities.Aggregates.Community.Event, EventDto>((List<Domain.Entities.Aggregates.Community.Event>)events);

            CacheHelper.AddToCache(_redisService, cacheKey, eventsDto);
        }

        return eventsDto;
    }
}
