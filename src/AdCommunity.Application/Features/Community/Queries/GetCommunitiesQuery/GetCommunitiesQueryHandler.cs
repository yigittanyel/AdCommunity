using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.Community.Queries.GetCommunitiesQuery;

public class GetCommunitiesQueryHandler : IYtRequestHandler<GetCommunitiesQuery, List<CommunityDto>>
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(1);
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public GetCommunitiesQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<CommunityDto>> Handle(GetCommunitiesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "communities";

        var communitiesDto = await _redisService.GetFromCacheAsync<List<CommunityDto>>(cacheKey);

        if (communitiesDto is null)
        {
            var communities = await _unitOfWork.CommunityRepository
                                .GetAllAsync(null,query => query.Include(x => x.User), cancellationToken);
            
            if (communities is null || !communities.Any())
            {
                throw new NotFoundException("Community",_httpContextAccessor.HttpContext);
            }

            communitiesDto = _mapper.MapList<Domain.Entities.Aggregates.Community.Community, CommunityDto>((List<Domain.Entities.Aggregates.Community.Community>)communities);

            await _redisService.AddToCacheAsync(cacheKey, communitiesDto, CacheTime);
        }

        return communitiesDto;
    }
}
