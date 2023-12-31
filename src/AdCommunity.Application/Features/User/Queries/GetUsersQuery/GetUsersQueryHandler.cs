﻿using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Helpers;
using  AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

namespace AdCommunity.Application.Features.User.Queries.GetUsersQuery;

public class GetUsersQueryHandler : IYtRequestHandler<GetUsersQuery, List<UserDto>>
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(1);
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;
    public GetUsersQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "users";

        var usersDto = await _redisService.GetFromCacheAsync<List<UserDto>>(cacheKey);

        if (usersDto is null)
        {
            var users = await _unitOfWork.GetRepository<UserRepository>().GetAllAsync(null, null, cancellationToken);

            if (users is null || !users.Any())
            {
                throw new NotFoundException("User");
            }

            usersDto = _mapper.MapList<Domain.Entities.Aggregates.User.User, UserDto>((List<Domain.Entities.Aggregates.User.User>)users);

            await _redisService.AddToCacheAsync(cacheKey, usersDto, CacheTime);
        }

        return usersDto;
    }
}
