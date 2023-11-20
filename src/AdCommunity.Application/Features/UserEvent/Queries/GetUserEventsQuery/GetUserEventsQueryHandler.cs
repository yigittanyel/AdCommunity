﻿using AdCommunity.Application.DTOs.UserEvent;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.UserEvent.Queries.GetUserEventsQuery;

public class GetUserEventsQueryHandler : IYtRequestHandler<GetUserEventsQuery, List<UserEventDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;

    public GetUserEventsQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<List<UserEventDto>> Handle(GetUserEventsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "userEvents";

        var userEventsDto = await _redisService.GetFromCacheAsync<List<UserEventDto>>(cacheKey);

        if (userEventsDto == null)
        {
            var userEvents = await _unitOfWork.UserEventRepository.GetAllAsync(null, query => query.Include(x => x.User).Include(x => x.Event), cancellationToken);

            if (userEvents == null || !userEvents.Any())
            {
                throw new NotFoundException("UserEvent");
            }

            userEventsDto = _mapper.MapList<Domain.Entities.Aggregates.User.UserEvent, UserEventDto>((List<Domain.Entities.Aggregates.User.UserEvent>)userEvents);

            await _redisService.AddToCacheAsync(cacheKey, userEventsDto, TimeSpan.FromMinutes(1));
        }

        return userEventsDto;
    }
}