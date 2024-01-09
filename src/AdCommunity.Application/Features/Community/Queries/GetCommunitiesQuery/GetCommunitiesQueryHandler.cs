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
    private readonly IElasticSearchService _elasticSearchService;

    public GetCommunitiesQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService, IElasticSearchService elasticSearchService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
        _elasticSearchService = elasticSearchService;
    }

    public async Task<List<CommunityDto>> Handle(GetCommunitiesQuery request, CancellationToken cancellationToken)
    {
        var indexName = "communities_index";
        var cacheKey = "communities";

        if (!await _elasticSearchService.IndexExistsAsync(indexName))
        {
            await _elasticSearchService.CreateIndexAsync(indexName, @"
                {
                    ""mappings"": {
                        ""properties"": {
                            ""JoinDate"": { ""type"": ""date"" },
                            ""UserId"": { ""type"": ""integer"" },
                            ""CommunityId"": { ""type"": ""integer"" }
                        }
                    }
                }");
        }

        // Try to get communities from Elasticsearch
        var elasticSearchCommunities = await _elasticSearchService.SearchAsync<CommunityDto>(indexName, "name", "value");

        if (elasticSearchCommunities.Any())
        {
            return elasticSearchCommunities;
        }

        // If not found in Elasticsearch, try to get from cache
        var communitiesDto = await _redisService.GetFromCacheAsync<List<CommunityDto>>(cacheKey);

        if (communitiesDto is null)
        {
            // If not found in cache, get from the database
            var communities = await _unitOfWork.GetRepository<CommunityRepository>()
                .GetAllAsync(null, query => query.Include(x => x.User), cancellationToken);

            if (communities is null || !communities.Any())
            {
                throw new NotFoundException("Community");
            }

            communitiesDto = _mapper.MapList<Domain.Entities.Aggregates.Community.Community, CommunityDto>((List<Domain.Entities.Aggregates.Community.Community>)communities);

            // Sync communities to Elasticsearch for future queries
            await _elasticSearchService.SyncToElastic(indexName, () => Task.FromResult(communitiesDto));

            // Add communities to cache
            await _redisService.AddToCacheAsync(cacheKey, communitiesDto, CacheTime);
        }

        return communitiesDto;
    }
}
