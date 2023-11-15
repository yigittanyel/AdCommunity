using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.Community.Queries.GetCommunitiesQuery;

public class GetCommunitiesQueryHandler : IYtRequestHandler<GetCommunitiesQuery, List<CommunityDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;

    public GetCommunitiesQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<List<CommunityDto>> Handle(GetCommunitiesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "communities";

        var communitiesDto = await _redisService.GetFromCacheAsync<List<CommunityDto>>(cacheKey);

        if (communitiesDto == null)
        {
            var communities = await _unitOfWork.CommunityRepository
                                .GetAllAsync(null,query => query.Include(x => x.User), cancellationToken);
            
            if (communities == null || !communities.Any())
            {
                throw new NotFoundException("Community");
            }

            communitiesDto = _mapper.MapList<Domain.Entities.Aggregates.Community.Community, CommunityDto>((List<Domain.Entities.Aggregates.Community.Community>)communities);

            await _redisService.AddToCacheAsync(cacheKey, communitiesDto, TimeSpan.FromMinutes(1));
        }

        return communitiesDto;
    }
}
