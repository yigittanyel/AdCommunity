using AdCommunity.Application.DTOs.TicketTypes;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.Ticket.Queries.GetTicketsQuery;

public class GetTicketTypeQueryHandler : IYtRequestHandler<GetTicketTypeQuery, TicketTypesDto>
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(1);
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;
    public GetTicketTypeQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }
    public async Task<TicketTypesDto> Handle(GetTicketTypeQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"ticket:{request.Id}";

        var ticketDto = await _redisService.GetFromCacheAsync<TicketTypesDto>(cacheKey);

        if (ticketDto is null)
        {
            var ticket = await _unitOfWork.GetRepository<TicketRepository>().
                GetAsync(request.Id, query => query.Include(x => x.Community).Include(x => x.CommunityEvent),
                cancellationToken);


            if (ticket is null)
            {
                throw new NotFoundException("community");
            }

            ticketDto = _mapper.Map<Domain.Entities.Aggregates.Community.TicketType, TicketTypesDto>(ticket);

            await _redisService.AddToCacheAsync(cacheKey, ticketDto, CacheTime);
        }

        return ticketDto;
    }
}
