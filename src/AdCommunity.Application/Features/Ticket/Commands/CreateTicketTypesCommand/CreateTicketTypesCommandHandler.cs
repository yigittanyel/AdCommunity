using AdCommunity.Application.DTOs.TicketTypes;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Features.Ticket.Commands.CreateTicketCommand;

public class CreateTicketTypesCommandHandler : IYtRequestHandler<CreateTicketTypesCommand, TicketTypesCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateTicketTypesCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<TicketTypesCreateDto> Handle(CreateTicketTypesCommand request, CancellationToken cancellationToken)
    {
        var existingTicket = await _unitOfWork.GetRepository<TicketRepository>().GetTicketByEventAndCommunityIdsAsync(request.CommunityEventId, request.CommunityId, cancellationToken);

        if (existingTicket is not null)
            throw new AlreadyExistsException("Ticket", _httpContextAccessor.HttpContext);

        var ticket = new Domain.Entities.Aggregates.Community.TicketType(request.Price);

        var communityEvent = await _unitOfWork.GetRepository<EventRepository>().GetAsync(request.CommunityEventId,null, cancellationToken);

        if (communityEvent is null)
            throw new NotExistException("Event",_httpContextAccessor.HttpContext);

        ticket.AssignEvent(communityEvent);

        var community = await _unitOfWork.GetRepository<CommunityRepository>().GetAsync(request.CommunityId,null, cancellationToken);

        if (community is null)
            throw new NotExistException("Community",_httpContextAccessor.HttpContext);

        ticket.AssignCommunity(community);

        community.AddTicket(ticket);
        
        _unitOfWork.GetRepository<CommunityRepository>().Update(community);

        _rabbitMqFactory.PublishMessage("create_ticket_queue", $"Ticket has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.Community.TicketType, TicketTypesCreateDto>(ticket);
    }
}
