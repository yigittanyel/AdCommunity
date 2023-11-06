using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Helpers;
using AdCommunity.Application.Services;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Community.Queries;

public class GetCommunitiesQuery:IYtRequest<List<CommunityDto>>
{
}

public class GetCommunitiesQueryHandler : IYtRequestHandler<GetCommunitiesQuery, List<CommunityDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly RedisService _redisService;

    public GetCommunitiesQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, RedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<List<CommunityDto>> Handle(GetCommunitiesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "communities";

        var communitiesDto = CacheHelper.GetFromCache<List<CommunityDto>>(_redisService, cacheKey);

        if (communitiesDto == null)
        {
            var communities = await _unitOfWork.CommunityRepository.GetAllAsync(cancellationToken);

            if (communities == null || !communities.Any())
            {
                throw new NotFoundException("Community");
            }

            communitiesDto = _mapper.MapList<AdCommunity.Domain.Entities.Aggregates.Community.Community, CommunityDto>((List<Domain.Entities.Aggregates.Community.Community>)communities);

            CacheHelper.AddToCache(_redisService, cacheKey, communitiesDto);
        }

        return communitiesDto;
    }
}
