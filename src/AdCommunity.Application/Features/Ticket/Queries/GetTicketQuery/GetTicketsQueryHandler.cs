using AdCommunity.Application.DTOs.Ticket;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Ticket.Queries.GetTicketQuery;

public class GetTicketsQueryHandler : IYtRequestHandler<GetTicketsQuery, List<TicketDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;

    public GetTicketsQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<List<TicketDto>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "tickets";

        var ticketsDto = await _redisService.GetFromCacheAsync<List<TicketDto>>(cacheKey);

        if (ticketsDto == null)
        {
            var tickets = await _unitOfWork.TicketRepository.GetAllAsync(cancellationToken);

            if (tickets == null || !tickets.Any())
            {
                throw new NotFoundException("Tickets");
            }

            ticketsDto = _mapper.MapList<Domain.Entities.Aggregates.Community.Ticket, TicketDto>((List<Domain.Entities.Aggregates.Community.Ticket>)tickets);

            await _redisService.AddToCacheAsync(cacheKey, ticketsDto, TimeSpan.FromMinutes(1));
        }

        return ticketsDto;
    }
}
