using AdCommunity.Application.DTOs.UserEvent;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.UserEvent.Queries.GetUserEventQuery;

public class GetUserEventQueryHandler : IYtRequestHandler<GetUserEventQuery, UserEventDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;

    public GetUserEventQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<UserEventDto> Handle(GetUserEventQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"userEvent:{request.Id}";

        var userEventDto = await _redisService.GetFromCacheAsync<UserEventDto>(cacheKey);

        if (userEventDto == null)
        {
            var userEvent = await _unitOfWork.UserEventRepository.GetAsync(request.Id, query => query.Include(x => x.User).Include(x => x.Event), cancellationToken);

            if (userEvent == null)
            {
                throw new NotFoundException("userEvent", request.Id);
            }

            userEventDto = _mapper.Map<Domain.Entities.Aggregates.User.UserEvent, UserEventDto>(userEvent);

            await _redisService.AddToCacheAsync(cacheKey, userEventDto, TimeSpan.FromMinutes(1));
        }

        return userEventDto;
    }
}
