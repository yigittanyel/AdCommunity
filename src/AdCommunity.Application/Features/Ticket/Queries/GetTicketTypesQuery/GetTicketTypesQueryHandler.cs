using AdCommunity.Application.DTOs.TicketTypes;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Ticket.Queries.GetTicketQuery;

public class GetTicketTypesQueryHandler : IYtRequestHandler<GetTicketTypesQuery, List<TicketTypesDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;

    public GetTicketTypesQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<List<TicketTypesDto>> Handle(GetTicketTypesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "tickets";

        var ticketsDto = await _redisService.GetFromCacheAsync<List<TicketTypesDto>>(cacheKey);

        if (ticketsDto == null)
        {
            var tickets = await _unitOfWork.TicketRepository.GetAllAsync(cancellationToken);

            if (tickets == null || !tickets.Any())
            {
                throw new NotFoundException("Tickets");
            }

            ticketsDto = _mapper.MapList<Domain.Entities.Aggregates.Community.TicketType, TicketTypesDto>((List<Domain.Entities.Aggregates.Community.TicketType>)tickets);

            await _redisService.AddToCacheAsync(cacheKey, ticketsDto, TimeSpan.FromMinutes(1));
        }

        return ticketsDto;
    }
}
