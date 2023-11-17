﻿using AdCommunity.Application.DTOs.UserCommunity;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.ElasticSearch;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AdCommunity.Application.Features.UserCommunity.Queries.GetUserCommunitiesQuery
{
    public class GetUserCommunitiesQueryHandler : IYtRequestHandler<GetUserCommunitiesQuery, List<UserCommunityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IYtMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IElasticSearchService _elasticSearchService;
        private const string IndexName = "user_communities";

        public GetUserCommunitiesQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService, IElasticSearchService elasticSearchService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _redisService = redisService;
            _elasticSearchService = elasticSearchService;
        }

        public async Task<List<UserCommunityDto>> Handle(GetUserCommunitiesQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "userCommunities";

            var userCommunitiesDto = await _redisService.GetFromCacheAsync<List<UserCommunityDto>>(cacheKey);

            // Elasticsearch index kontrolü ve oluşturma
            if (!await _elasticSearchService.IndexExistsAsync(IndexName))
            {
                await _elasticSearchService.CreateIndexAsync(IndexName, @"
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

            if (userCommunitiesDto == null || userCommunitiesDto.Count == 0)
            {
                // Elasticsearch'te veri yoksa veritabanından al
                var userCommunities = await _unitOfWork.UserCommunityRepository
                    .GetAllAsync(null, query => query.Include(x => x.Community).Include(x => x.User),
                        cancellationToken);

                if (userCommunities == null || !userCommunities.Any())
                {
                    throw new NotFoundException("UserCommunity");
                }

                userCommunitiesDto = _mapper.MapList<Domain.Entities.Aggregates.User.UserCommunity, UserCommunityDto>(userCommunities.ToList());

                // Elasticsearch'e ekleyerek cache'i güncelle
                foreach (var userCommunity in userCommunitiesDto)
                {
                    await _elasticSearchService.SyncSingleToElastic<UserCommunityDto>(IndexName, userCommunity);
                }

                await _redisService.AddToCacheAsync(cacheKey, userCommunitiesDto, TimeSpan.FromMinutes(1));
            }

            return userCommunitiesDto;
        }
    }
}
