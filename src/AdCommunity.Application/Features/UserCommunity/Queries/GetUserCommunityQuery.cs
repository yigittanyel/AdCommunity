using AdCommunity.Application.DTOs.UserCommunity;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.UserCommunity.Queries;

public class GetUserCommunityQuery:IYtRequest<UserCommunityDto>
{
    public int Id { get; set; }
}

public class GetUserCommunityQueryHandler : IYtRequestHandler<GetUserCommunityQuery, UserCommunityDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;

    public GetUserCommunityQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }


    public async Task<UserCommunityDto> Handle(GetUserCommunityQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"userCommunity:{request.Id}";

        var userCommunityDto = await _redisService.GetFromCacheAsync<UserCommunityDto>(cacheKey);

        if (userCommunityDto == null)
        {
            var userCommunity = await _unitOfWork.UserCommunityRepository.GetAsync(request.Id, cancellationToken);

            if (userCommunity == null)
            {
                throw new NotFoundException("userCommunity", request.Id);
            }

            userCommunityDto = _mapper.Map<AdCommunity.Domain.Entities.Aggregates.User.UserCommunity, UserCommunityDto>(userCommunity);

            await _redisService.AddToCacheAsync(cacheKey, userCommunityDto,TimeSpan.FromMinutes(1));
        }

        return userCommunityDto;
    }
}
