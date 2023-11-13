using AdCommunity.Application.DTOs.UserCommunity;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.UserCommunity.Queries.GetUserCommunitiesQuery;

public class GetUserCommunitiesQueryHandler : IYtRequestHandler<GetUserCommunitiesQuery, List<UserCommunityDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;

    public GetUserCommunitiesQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<List<UserCommunityDto>> Handle(GetUserCommunitiesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "userCommunities";

        var userCommunitiesDto = await _redisService.GetFromCacheAsync<List<UserCommunityDto>>(cacheKey);

        if (userCommunitiesDto == null)
        {
            var userCommunities = await _unitOfWork.UserCommunityRepository.GetAllAsync(cancellationToken);

            if (userCommunities == null || !userCommunities.Any())
            {
                throw new NotFoundException("UserCommunity");
            }

            userCommunitiesDto = _mapper.MapList<Domain.Entities.Aggregates.User.UserCommunity, UserCommunityDto>((List<Domain.Entities.Aggregates.User.UserCommunity>)userCommunities);

            await _redisService.AddToCacheAsync(cacheKey, userCommunitiesDto, TimeSpan.FromMinutes(1));
        }

        return userCommunitiesDto;
    }
}
