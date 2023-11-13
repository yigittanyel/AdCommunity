using AdCommunity.Application.DTOs.Ticket;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Ticket.Queries.GetTicketsQuery;

public class GetTicketQueryHandler : IYtRequestHandler<GetTicketQuery, TicketDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;

    public GetTicketQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }
    public async Task<TicketDto> Handle(GetTicketQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"ticket:{request.Id}";

        var ticketDto = await _redisService.GetFromCacheAsync<TicketDto>(cacheKey);

        if (ticketDto == null)
        {
            var ticket = await _unitOfWork.TicketRepository.GetAsync(request.Id, cancellationToken);

            if (ticket == null)
            {
                throw new NotFoundException("community", request.Id);
            }

            ticketDto = _mapper.Map<Domain.Entities.Aggregates.Community.Ticket, TicketDto>(ticket);

            await _redisService.AddToCacheAsync(cacheKey, ticketDto, TimeSpan.FromMinutes(1));
        }

        return ticketDto;
    }
}
