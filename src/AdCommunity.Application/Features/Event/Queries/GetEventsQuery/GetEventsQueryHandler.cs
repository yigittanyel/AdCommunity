﻿using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Helpers;
using  AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.Event.Queries.GetEventsQuery;
public class GetEventsQueryHandler : IYtRequestHandler<GetEventsQuery, List<EventDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;
    public GetEventsQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<List<EventDto>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "events";

        var eventsDto = await _redisService.GetFromCacheAsync<List<EventDto>>(cacheKey);

        if (eventsDto is null)
        {
            var events = await _unitOfWork.GetRepository<EventRepository>().GetAllAsync(null,query=>query.Include(x=>x.Community),cancellationToken);

            if (events is null || !events.Any())
                throw new NotExistException("Event");


            eventsDto = _mapper.MapList<Domain.Entities.Aggregates.Community.Event, EventDto>((List<Domain.Entities.Aggregates.Community.Event>)events);

            await _redisService.AddToCacheAsync(cacheKey, eventsDto);
        }

        return eventsDto;
    }
}
