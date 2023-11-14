using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Community.Queries.GetCommunityQuery;

public class GetCommunityQueryHandler : IYtRequestHandler<GetCommunityQuery, CommunityDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;

    public GetCommunityQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<CommunityDto> Handle(GetCommunityQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"community:{request.Id}";

        var communityDto = await _redisService.GetFromCacheAsync<CommunityDto>(cacheKey);

        if (communityDto == null)
        {
            var community = await _unitOfWork.CommunityRepository.GetAsync(request.Id, null, cancellationToken);

            if (community == null)
            {
                throw new NotFoundException("community", request.Id);
            }

            communityDto = _mapper.Map<Domain.Entities.Aggregates.Community.Community, CommunityDto>(community);

            await _redisService.AddToCacheAsync(cacheKey, communityDto, TimeSpan.FromMinutes(1));
        }

        return communityDto;
    }
}
