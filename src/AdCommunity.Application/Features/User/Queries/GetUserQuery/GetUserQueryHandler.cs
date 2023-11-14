using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.User.Queries.GetUserQuery;

public class GetUserQueryHandler : IYtRequestHandler<GetUserQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;

    public GetUserQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"user:{request.Id}";

        var userDto = await _redisService.GetFromCacheAsync<UserDto>(cacheKey);

        if (userDto == null)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(request.Id, null, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("User", request.Id);
            }

            userDto = _mapper.Map<Domain.Entities.Aggregates.User.User, UserDto>(user);

            await _redisService.AddToCacheAsync(cacheKey, userDto, TimeSpan.FromMinutes(1));
        }

        return userDto;
    }
}
