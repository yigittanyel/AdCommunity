using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.Community.Queries.GetCommunityQuery;

public class GetCommunityQueryHandler : IYtRequestHandler<GetCommunityQuery, CommunityDto>
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(1);
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetCommunityQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CommunityDto> Handle(GetCommunityQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"community:{request.Id}";

        var communityDto = await _redisService.GetFromCacheAsync<CommunityDto>(cacheKey);

        if (communityDto is null)
        {
            var community = await _unitOfWork.GetRepository<CommunityRepository>()
                .GetAsync(request.Id, 
                    query=>query.Include(x=>x.User),
                    cancellationToken);

            if (community is null)
            {
                throw new NotFoundException("community", _httpContextAccessor.HttpContext);
            }

            communityDto = _mapper.Map<Domain.Entities.Aggregates.Community.Community, CommunityDto>(community);

            await _redisService.AddToCacheAsync(cacheKey, communityDto, CacheTime);
        }

        return communityDto;
    }
}
