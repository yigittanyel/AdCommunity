using AdCommunity.Application.DTOs.UserTicket;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Helpers;
using  AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.UserTicket.Queries.GetUserTicketQuery;
public class GetUserTicketQueryHandler : IYtRequestHandler<GetUserTicketQuery, UserTicketDto>
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(1);
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;
    private readonly LocalizationService _localizationService;
    public GetUserTicketQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService, LocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
        _localizationService = localizationService;
    }

    public async Task<UserTicketDto> Handle(GetUserTicketQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"userTicket:{request.Id}";

        var userTicketDto = await _redisService.GetFromCacheAsync<UserTicketDto>(cacheKey);

        if (userTicketDto is null)
        {
            var userTicket = await _unitOfWork.GetRepository<UserTicketRepository>().GetAsync(request.Id, query => query.Include(x => x.User).Include(x => x.Ticket), cancellationToken);

            if (userTicket is null)
                throw new NotExistException(_localizationService, "User Ticket");

            userTicketDto = _mapper.Map<Domain.Entities.Aggregates.User.UserTicket, UserTicketDto>(userTicket);

            await _redisService.AddToCacheAsync(cacheKey, userTicketDto, CacheTime);
        }

        return userTicketDto;
    }
}
