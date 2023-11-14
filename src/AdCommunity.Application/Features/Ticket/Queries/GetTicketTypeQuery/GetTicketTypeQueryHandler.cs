using AdCommunity.Application.DTOs.TicketTypes;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Ticket.Queries.GetTicketsQuery;

public class GetTicketTypeQueryHandler : IYtRequestHandler<GetTicketTypeQuery, TicketTypesDto>
{
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

        if (ticketDto == null)
        {
            var ticket = await _unitOfWork.TicketRepository.GetAsync(request.Id, null, cancellationToken);

            if (ticket == null)
            {
                throw new NotFoundException("community", request.Id);
            }

            ticketDto = _mapper.Map<Domain.Entities.Aggregates.Community.TicketType, TicketTypesDto>(ticket);

            await _redisService.AddToCacheAsync(cacheKey, ticketDto, TimeSpan.FromMinutes(1));
        }

        return ticketDto;
    }
}
