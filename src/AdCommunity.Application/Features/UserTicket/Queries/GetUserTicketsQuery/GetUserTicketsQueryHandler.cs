﻿using AdCommunity.Application.DTOs.UserTicket;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace AdCommunity.Application.Features.UserTicket.Queries.GetUserTicketsQuery;
public class GetUserTicketsQueryHandler : IYtRequestHandler<GetUserTicketsQuery, List<UserTicketDto>>
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(1);
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;
    private readonly IStringLocalizerFactory _localizer;
    public GetUserTicketsQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService, IStringLocalizerFactory localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
        _localizer = localizer;
    }

    public async Task<List<UserTicketDto>> Handle(GetUserTicketsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "userTickets";

        var userTicketsDto = await _redisService.GetFromCacheAsync<List<UserTicketDto>>(cacheKey);

        if (userTicketsDto is null)
        {
            var userTickets = await _unitOfWork.GetRepository<UserTicketRepository>().GetAllAsync(null, query=>query.Include(x=>x.User).Include(x=>x.Ticket), cancellationToken);

            if (userTickets is null || !userTickets.Any())
                throw new NotExistException((IStringLocalizer)_localizer, "User Ticket");

            userTicketsDto = _mapper.MapList<Domain.Entities.Aggregates.User.UserTicket, UserTicketDto>((List<Domain.Entities.Aggregates.User.UserTicket>)userTickets);

            await _redisService.AddToCacheAsync(cacheKey, userTicketsDto, CacheTime);
        }

        return userTicketsDto;
    }
}
