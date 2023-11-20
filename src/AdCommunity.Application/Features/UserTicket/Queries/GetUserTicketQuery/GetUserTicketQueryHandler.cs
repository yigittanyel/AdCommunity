using AdCommunity.Application.DTOs.UserTicket;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.UserTicket.Queries.GetUserTicketQuery;

public class GetUserTicketQueryHandler : IYtRequestHandler<GetUserTicketQuery, UserTicketDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;

    public GetUserTicketQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<UserTicketDto> Handle(GetUserTicketQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"userTicket:{request.Id}";

        var userTicketDto = await _redisService.GetFromCacheAsync<UserTicketDto>(cacheKey);

        if (userTicketDto == null)
        {
            var userTicket = await _unitOfWork.UserTicketRepository.GetAsync(request.Id, query => query.Include(x => x.User).Include(x => x.Ticket), cancellationToken);

            if (userTicket == null)
            {
                throw new NotFoundException("userTicket", request.Id);
            }

            userTicketDto = _mapper.Map<Domain.Entities.Aggregates.User.UserTicket, UserTicketDto>(userTicket);

            await _redisService.AddToCacheAsync(cacheKey, userTicketDto, TimeSpan.FromMinutes(1));
        }

        return userTicketDto;
    }
}
