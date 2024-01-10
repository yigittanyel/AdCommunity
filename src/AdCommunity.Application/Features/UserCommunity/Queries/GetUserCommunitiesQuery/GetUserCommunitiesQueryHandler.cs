using AdCommunity.Application.DTOs.UserCommunity;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.UserCommunity.Queries.GetUserCommunitiesQuery
{
    public class GetUserCommunitiesQueryHandler : IYtRequestHandler<GetUserCommunitiesQuery, List<UserCommunityDto>>
    {
        private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(1);
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

            if (userCommunitiesDto is null || userCommunitiesDto.Count == 0)
            {
                // Elasticsearch'te veri yoksa veritabanından al
                var userCommunities = await _unitOfWork.GetRepository<UserCommunityRepository>()
                    .GetAllAsync(null, query => query.Include(x => x.Community).Include(x => x.User),
                        cancellationToken);

                if (userCommunities is null || !userCommunities.Any())
                {
                    throw new NotExistException("UserCommunity");
                }

                userCommunitiesDto = _mapper.MapList<Domain.Entities.Aggregates.User.UserCommunity, UserCommunityDto>(userCommunities.ToList());

                await _redisService.AddToCacheAsync(cacheKey, userCommunitiesDto, CacheTime);
            }

            return userCommunitiesDto;
        }
    }
}
