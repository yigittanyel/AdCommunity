using AdCommunity.Application.DTOs.TicketTypes;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Ticket.Commands.CreateTicketCommand;

public class CreateTicketTypesCommandHandler : IYtRequestHandler<CreateTicketTypesCommand, TicketTypesCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;

    public CreateTicketTypesCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }
    public async Task<TicketTypesCreateDto> Handle(CreateTicketTypesCommand request, CancellationToken cancellationToken)
    {
        var existingTicket = await _unitOfWork.TicketRepository.GetTicketByEventAndCommunityIdsAsync(request.CommunityEventId, request.CommunityId, cancellationToken);

        if (existingTicket is not null)
            throw new AlreadyExistsException("Ticket");

        var ticket = new Domain.Entities.Aggregates.Community.TicketType(request.Price);

        var communityEvent = await _unitOfWork.EventRepository.GetAsync(request.CommunityEventId,null, cancellationToken);

        if (communityEvent is null)
            throw new NotExistException("Event");

        ticket.AssignEvent(communityEvent);

        var community = await _unitOfWork.CommunityRepository.GetAsync(request.CommunityId,null, cancellationToken);

        if (community is null)
            throw new NotExistException("Community");

        ticket.AssignCommunity(community);

        community.AddTicket(ticket);
        
        _unitOfWork.CommunityRepository.Update(community);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("create_ticket_queue", $"Ticket has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.Community.TicketType, TicketTypesCreateDto>(ticket);
    }
}
