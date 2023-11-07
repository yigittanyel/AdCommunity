using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Event.Queries.GetEventQuery;

public class GetEventQueryHandler : IYtRequestHandler<GetEventQuery, EventDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;

    public GetEventQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<EventDto> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"event:{request.Id}";

        var eventDto = await _redisService.GetFromCacheAsync<EventDto>(cacheKey);

        if (eventDto == null)
        {
            var _event = await _unitOfWork.EventRepository.GetAsync(request.Id, cancellationToken);

            if (_event == null)
            {
                throw new NotFoundException("event", request.Id);
            }

            eventDto = _mapper.Map<Domain.Entities.Aggregates.Community.Event, EventDto>(_event);

            await _redisService.AddToCacheAsync(cacheKey, eventDto, TimeSpan.FromMinutes(1));
        }

        return eventDto;
    }
}
