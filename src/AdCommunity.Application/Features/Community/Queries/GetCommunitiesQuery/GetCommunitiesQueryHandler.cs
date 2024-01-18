using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.ElasticSearch;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.Community.Queries.GetCommunitiesQuery;

public class GetCommunitiesQueryHandler : IYtRequestHandler<GetCommunitiesQuery, List<CommunityDto>>
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(1);
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;

    public GetCommunitiesQueryHandler(
        IUnitOfWork unitOfWork,
        IYtMapper mapper,
        IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<List<CommunityDto>> Handle(GetCommunitiesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "communities";
        var indexName = "communities_index";
        var communitiesDto = await _redisService.GetFromCacheAsync<List<CommunityDto>>(cacheKey);

        //if (communitiesDto is null)
        //{
        //    await _elasticSearchService.CheckIndex(indexName, descriptor =>
        //        descriptor.CommunityMapping()
        //    );

        //    var elasticCommunities = await _elasticSearchService.GetDocuments(indexName);

        //    if (elasticCommunities == null || !elasticCommunities.Any())
        //    {
        //        var communities = await _unitOfWork.GetRepository<CommunityRepository>()
        //                                           .GetAllAsync(null, query => query.Include(x => x.User), cancellationToken);

        //        if (communities == null || !communities.Any())
        //        {
        //            throw new NotFoundException("Community");
        //        }
        //        communitiesDto = _mapper.MapList<Domain.Entities.Aggregates.Community.Community, CommunityDto>((List<Domain.Entities.Aggregates.Community.Community>)communities);
        //        var domainCommunities = _mapper.MapList<CommunityDto, AdCommunity.Domain.Entities.Aggregates.Community.Community>(communitiesDto);

        //        await _elasticSearchService.InsertBulkDocuments(indexName, domainCommunities);

        //        await _redisService.AddToCacheAsync(cacheKey, communitiesDto, CacheTime);
        //    }
        //    else
        //    {
        //        communitiesDto = _mapper.MapList<AdCommunity.Domain.Entities.Aggregates.Community.Community, CommunityDto>(elasticCommunities);
        //        await _redisService.AddToCacheAsync(cacheKey, communitiesDto, CacheTime);
        //    }
        //}

        return communitiesDto;
    }
}
