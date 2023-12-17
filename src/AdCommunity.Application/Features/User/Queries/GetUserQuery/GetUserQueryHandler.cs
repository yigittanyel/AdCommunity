using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.Extensions.Localization;

namespace AdCommunity.Application.Features.User.Queries.GetUserQuery;

public class GetUserQueryHandler : IYtRequestHandler<GetUserQuery, UserDto>
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(1);
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;
    private readonly IStringLocalizerFactory _localizer;
    public GetUserQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService, IStringLocalizerFactory localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
        _localizer = localizer;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"user:{request.Id}";

        var userDto = await _redisService.GetFromCacheAsync<UserDto>(cacheKey);

        if (userDto is null)
        {
            var user = await _unitOfWork.GetRepository<UserRepository>().GetAsync(request.Id, null, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException((IStringLocalizer)_localizer, "User");
            }

            userDto = _mapper.Map<Domain.Entities.Aggregates.User.User, UserDto>(user);

            await _redisService.AddToCacheAsync(cacheKey, userDto, CacheTime);
        }

        return userDto;
    }
}
