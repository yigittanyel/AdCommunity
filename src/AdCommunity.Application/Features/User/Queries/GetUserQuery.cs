using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Helpers;
using AdCommunity.Application.Services;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.User.Queries;

public class GetUserQuery : IYtRequest<UserDto>
{
    public int Id { get; set; }
}

public class GetUserQueryHandler : IYtRequestHandler<GetUserQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly RedisService _redisService;

    public GetUserQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, RedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"user:{request.Id}";

        var userDto = CacheHelper.GetFromCache<UserDto>(_redisService, cacheKey);

        if (userDto == null)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(request.Id, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("User", request.Id);
            }

            userDto = _mapper.Map<AdCommunity.Domain.Entities.Aggregates.User.User, UserDto>(user);

            CacheHelper.AddToCache(_redisService, cacheKey, userDto);
        }

        return userDto;
    }
}
