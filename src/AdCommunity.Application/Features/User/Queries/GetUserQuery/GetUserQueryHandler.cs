using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Helpers;
using  AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

namespace AdCommunity.Application.Features.User.Queries.GetUserQuery;

public class GetUserQueryHandler : IYtRequestHandler<GetUserQuery, UserDto>
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(1);
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;
    private readonly LocalizationService _localizationService;
    public GetUserQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService, LocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
        _localizationService = localizationService;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"user:{request.Id}";

        var userDto = await _redisService.GetFromCacheAsync<UserDto>(cacheKey);

        if (userDto is null)
        {
            var user = await _unitOfWork.GetRepository<UserRepository>().GetAsync(request.Id, null, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException(_localizationService, "User");
            }

            userDto = _mapper.Map<Domain.Entities.Aggregates.User.User, UserDto>(user);

            await _redisService.AddToCacheAsync(cacheKey, userDto, CacheTime);
        }

        return userDto;
    }
}
