using AdCommunity.Application.DTOs.UserTicket;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Helpers;
using  AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.UserTicket.Queries.GetUserTicketsQuery;
public class GetUserTicketsQueryHandler : IYtRequestHandler<GetUserTicketsQuery, List<UserTicketDto>>
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(1);
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;
    private readonly LocalizationService _localizationService;
    public GetUserTicketsQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService, LocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
        _localizationService = localizationService;
    }

    public async Task<List<UserTicketDto>> Handle(GetUserTicketsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "userTickets";

        var userTicketsDto = await _redisService.GetFromCacheAsync<List<UserTicketDto>>(cacheKey);

        if (userTicketsDto is null)
        {
            var userTickets = await _unitOfWork.GetRepository<UserTicketRepository>().GetAllAsync(null, query=>query.Include(x=>x.User).Include(x=>x.Ticket), cancellationToken);

            if (userTickets is null || !userTickets.Any())
                throw new NotExistException(_localizationService, "User Ticket");

            userTicketsDto = _mapper.MapList<Domain.Entities.Aggregates.User.UserTicket, UserTicketDto>((List<Domain.Entities.Aggregates.User.UserTicket>)userTickets);

            await _redisService.AddToCacheAsync(cacheKey, userTicketsDto, CacheTime);
        }

        return userTicketsDto;
    }
}
