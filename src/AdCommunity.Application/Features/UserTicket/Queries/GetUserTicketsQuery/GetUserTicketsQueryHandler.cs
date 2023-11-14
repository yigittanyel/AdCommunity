﻿using AdCommunity.Application.DTOs.UserTicket;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.UserTicket.Queries.GetUserTicketsQuery;


public class GetUserTicketsQueryHandler : IYtRequestHandler<GetUserTicketsQuery, List<UserTicketDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;

    public GetUserTicketsQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<List<UserTicketDto>> Handle(GetUserTicketsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "userTickets";

        var userTicketsDto = await _redisService.GetFromCacheAsync<List<UserTicketDto>>(cacheKey);

        if (userTicketsDto == null)
        {
            var userTickets = await _unitOfWork.UserTicketRepository.GetAllAsync(null, null, cancellationToken);

            if (userTickets == null || !userTickets.Any())
            {
                throw new NotFoundException("UserTicket");
            }

            userTicketsDto = _mapper.MapList<Domain.Entities.Aggregates.User.UserTicket, UserTicketDto>((List<Domain.Entities.Aggregates.User.UserTicket>)userTickets);

            await _redisService.AddToCacheAsync(cacheKey, userTicketsDto, TimeSpan.FromMinutes(1));
        }

        return userTicketsDto;
    }
}
