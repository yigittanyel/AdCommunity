using AdCommunity.Application.DTOs.UserCommunity;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Helpers;
using  AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.UserCommunity.Queries.GetUserCommunityQuery;

public class GetUserCommunityQueryHandler : IYtRequestHandler<GetUserCommunityQuery, UserCommunityDto>
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(1);
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;
    private readonly LocalizationService _localizationService;
    public GetUserCommunityQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService, LocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
        _localizationService = localizationService;
    }

    public async Task<UserCommunityDto> Handle(GetUserCommunityQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"userCommunity:{request.Id}";

        var userCommunityDto = await _redisService.GetFromCacheAsync<UserCommunityDto>(cacheKey);

        if (userCommunityDto is null)
        {
            var userCommunity = await _unitOfWork.GetRepository<UserCommunityRepository>().
                GetAsync(request.Id, query => query.Include(x => x.Community).Include(x => x.User),
                cancellationToken);

            if (userCommunity is null)
                throw new NotExistException(_localizationService, "UserCommunity");

            userCommunityDto = _mapper.Map<Domain.Entities.Aggregates.User.UserCommunity, UserCommunityDto>(userCommunity);

            await _redisService.AddToCacheAsync(cacheKey, userCommunityDto, CacheTime);
        }

        return userCommunityDto;
    }
}
