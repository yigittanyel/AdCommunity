using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Helpers;
using AdCommunity.Application.Services;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Community.Queries;

public class GetCommunityQuery:IYtRequest<CommunityDto>
{
    public int Id { get; set; }
}

public class GetCommunityQueryHandler : IYtRequestHandler<GetCommunityQuery, CommunityDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly RedisService _redisService;

    public GetCommunityQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, RedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<CommunityDto> Handle(GetCommunityQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"community:{request.Id}";

        var communityDto = CacheHelper.GetFromCache<CommunityDto>(_redisService, cacheKey);

        if (communityDto == null)
        {
            var community = await _unitOfWork.CommunityRepository.GetAsync(request.Id, cancellationToken);

            if (community == null)
            {
                throw new NotFoundException("community", request.Id);
            }

            communityDto = _mapper.Map<AdCommunity.Domain.Entities.Aggregates.Community.Community, CommunityDto>(community);

            CacheHelper.AddToCache(_redisService, cacheKey, communityDto);
        }

        return communityDto;
    }
}
