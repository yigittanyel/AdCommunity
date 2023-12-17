using AdCommunity.Application.DTOs.TicketTypes;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.Ticket.Queries.GetTicketQuery;

public class GetTicketTypesQueryHandler : IYtRequestHandler<GetTicketTypesQuery, List<TicketTypesDto>>
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(1);
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetTicketTypesQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<TicketTypesDto>> Handle(GetTicketTypesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "tickets";

        var ticketsDto = await _redisService.GetFromCacheAsync<List<TicketTypesDto>>(cacheKey);

        if (ticketsDto is null)
        {
            var tickets = await _unitOfWork.GetRepository<TicketRepository>().
                GetAllAsync(null,query=>query.Include(x=>x.Community).Include(x=>x.CommunityEvent),
                cancellationToken);

            if (tickets is null || !tickets.Any())
            {
                throw new NotFoundException("Tickets",_httpContextAccessor.HttpContext);
            }

            ticketsDto = _mapper.MapList<Domain.Entities.Aggregates.Community.TicketType, TicketTypesDto>((List<Domain.Entities.Aggregates.Community.TicketType>)tickets);

            await _redisService.AddToCacheAsync(cacheKey, ticketsDto, CacheTime);
        }

        return ticketsDto;
    }
}
